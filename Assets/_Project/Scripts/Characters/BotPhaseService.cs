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
        
        private int _hardInstanceId;
        private int _bossInstanceId;
        
        private int _enemiesKilledUntilHardPhase;
        private int _enemiesKilledUntilBossPhase;
        private bool _enemiesKilledUntilHardPhaseFrozen;
        private bool _enemiesKilledUntilBossPhaseFrozen;
        
        private BotPhase _phase = BotPhase.Default;

        public BotPhase Phase => _phase;
        public int EnemiesKilledUntilHardPhase => _enemiesKilledUntilHardPhase;
        public int EnemiesKilledUntilBossPhase => _enemiesKilledUntilBossPhase;


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
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);
        }

        public void Dispose() => _lifetimeDefinition.Terminate();

        public bool TryGetHardInstanceId(out int instanceId)
        {
            instanceId = _hardInstanceId;
            return _hardInstanceId != 0;
        }

        public bool TryGetBossInstanceId(out int instanceId)
        {
            instanceId = _bossInstanceId;
            return _bossInstanceId != 0;
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
            _characterCreator.CreateOnRandomPos(definitionId);
            _hardInstanceId = TakeSpawnedInstanceId(definitionId);
        }

        public void SpawnBoss()
        {
            if (_phase != BotPhase.Boss || _bossInstanceId != 0)
                return;

            var bots = _charactersStorage.GetCharactersByTeam(Team.Bot).ToList();

            foreach (var victim in bots)
            {
                _healthChangeService.Enqueue(HealthChangeRequest.Damage(
                    HealthChangeSource.None,
                    victim,
                    victim.Health + victim.BonusHealth));
            }

            _healthChangeService.Apply();

            var definitionId = _botPhaseCharactersConfig.GetRandomBoss();
            _characterCreator.CreateOnRandomPos(definitionId);
            _bossInstanceId = TakeSpawnedInstanceId(definitionId);
        }

        private void OnCharacterDied(CharacterDiedEvent e)
        {
            if (e.Character.Team != Team.Player && e.Source.OwnerTeam == Team.Player)
                RegisterPlayerKill(e.Character.DefinitionId);

            if (e.Character.InstanceId == _bossInstanceId)
            {
                _bossInstanceId = 0;
                _phase = BotPhase.Default;
                _enemiesKilledUntilHardPhaseFrozen = false;
                _enemiesKilledUntilBossPhaseFrozen = false;
                _eventBus.Publish(new BotBossPhaseEndedEvent());
                return;
            }

            if (e.Character.InstanceId != _hardInstanceId)
                return;

            _hardInstanceId = 0;

            if (_phase == BotPhase.Boss)
                return;

            _phase = BotPhase.Default;
            _enemiesKilledUntilHardPhaseFrozen = false;
            _eventBus.Publish(new BotHardPhaseEndedEvent());
        }

        private void RegisterPlayerKill(string definitionId)
        {
            var listedPhase = _botPhaseCharactersConfig.RequireListedPhase(definitionId);

            if (listedPhase == BotPhase.Default && !_enemiesKilledUntilHardPhaseFrozen)
            {
                _enemiesKilledUntilHardPhase++;

                if (_enemiesKilledUntilHardPhase >= _botPhaseCharactersConfig.BotHardPhaseThreshold)
                {
                    _enemiesKilledUntilHardPhase = 0;
                    _enemiesKilledUntilHardPhaseFrozen = true;
                    _phase = BotPhase.Hard;
                }
            }
            else if (listedPhase == BotPhase.Hard && !_enemiesKilledUntilBossPhaseFrozen)
            {
                _enemiesKilledUntilBossPhase++;

                if (_enemiesKilledUntilBossPhase >= _botPhaseCharactersConfig.BotBossPhaseThreshold)
                {
                    _enemiesKilledUntilBossPhase = 0;
                    _enemiesKilledUntilHardPhaseFrozen = true;
                    _enemiesKilledUntilBossPhaseFrozen = true;
                    _phase = BotPhase.Boss;
                }
            }
        }

        private int TakeSpawnedInstanceId(string definitionId) =>
            _charactersStorage
                .GetCharactersByTeam(Team.Bot)
                .Single(character => character.DefinitionId == definitionId)
                .InstanceId;
    }
}
