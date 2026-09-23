using System;
using System.Linq;
using _Project.Scripts.Characters.Health;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.Characters
{
    public class BotPhaseService : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersStorage _charactersStorage;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly HealthChangeService _healthChangeService;
        private readonly BotPhaseCharactersConfig _botPhaseCharactersConfig;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private BotPhase _phase = BotPhase.Default;
        private int _hardInstanceId;
        private bool _capturingSpawn;
        private string _pendingHardDefinitionId;

        public BotPhase Phase => _phase;


        public BotPhaseService(
            EventBus eventBus,
            CharacterCreator characterCreator,
            CharactersStorage charactersStorage,
            InitialGameplayConfig initialGameplayConfig,
            HealthChangeService healthChangeService,
            BotPhaseCharactersConfig botPhaseCharactersConfig)
        {
            _eventBus = eventBus;
            _characterCreator = characterCreator;
            _charactersStorage = charactersStorage;
            _initialGameplayConfig = initialGameplayConfig;
            _healthChangeService = healthChangeService;
            _botPhaseCharactersConfig = botPhaseCharactersConfig;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<BossSpawnThresholdReachedEvent>(
                _lifetimeDefinition.Lifetime,
                OnBossSpawnThresholdReached);

            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);

            _eventBus.SubscribeWithLifetime<CharacterCreatedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterCreated);
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        public bool TryGetHardInstanceId(out int instanceId)
        {
            instanceId = _hardInstanceId;
            return _hardInstanceId != 0;
        }

        public void SpawnHardBot()
        {
            if (_phase != BotPhase.Hard || _hardInstanceId != 0)
                return;

            var bots = _charactersStorage.GetCharactersByTeam(Team.Bot).ToList();

            if (bots.Count >= _initialGameplayConfig.MaxCharactersCount)
            {
                var victim = bots[UnityEngine.Random.Range(0, bots.Count)];
                
                _healthChangeService.Enqueue(HealthChangeRequest.Damage(
                    HealthChangeSource.None,
                    victim,
                    victim.Health + victim.BonusHealth));
                
                _healthChangeService.Apply();
            }

            var definitionId = _botPhaseCharactersConfig.GetRandomHardBot();
            _pendingHardDefinitionId = definitionId;
            _capturingSpawn = true;

            try
            {
                _characterCreator.CreateOnRandomPos(definitionId);
            }
            finally
            {
                _capturingSpawn = false;
                _pendingHardDefinitionId = null;
            }
        }

        private void OnBossSpawnThresholdReached(BossSpawnThresholdReachedEvent e) =>
            _phase = BotPhase.Hard;

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Character.InstanceId != _hardInstanceId)
                return;

            _hardInstanceId = 0;
            _phase = BotPhase.Default;
            _eventBus.Publish(new BotHardPhaseEndedEvent());
        }

        private void OnCharacterCreated(CharacterCreatedEvent e)
        {
            if (!_capturingSpawn || e.Character.DefinitionId != _pendingHardDefinitionId)
                return;

            _hardInstanceId = e.Character.InstanceId;
        }
    }
}
