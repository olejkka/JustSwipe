using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Movement;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.Core;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States
{
    public class BotHardPhaseTurnState : State
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly BotSpawnChancesConfig _botSpawnChancesConfig;
        private readonly InitialGameplayConfig _initialGameplayConfig;
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersTurnOrchestrator _charactersTurnOrchestrator;
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersStorage _charactersStorage;
        private readonly BotPhaseService _botPhaseService;


        public BotHardPhaseTurnState(
            IReadOnlyList<ITransition> transitions,
            EventBus.EventBus eventBus,
            BotSpawnChancesConfig botSpawnChancesConfig,
            InitialGameplayConfig initialGameplayConfig,
            BotMoveCreator botMoveCreator,
            CharactersTurnOrchestrator charactersTurnOrchestrator,
            CharacterCreator characterCreator,
            CharactersStorage charactersStorage,
            BotPhaseService botPhaseService) : base(transitions)
        {
            _eventBus = eventBus;
            _botSpawnChancesConfig = botSpawnChancesConfig;
            _initialGameplayConfig = initialGameplayConfig;
            _botMoveCreator = botMoveCreator;
            _charactersTurnOrchestrator = charactersTurnOrchestrator;
            _characterCreator = characterCreator;
            _charactersStorage = charactersStorage;
            _botPhaseService = botPhaseService;
        }

        protected override void OnEnter()
        {
            var direction = _botPhaseService.TryGetHardInstanceId(out var instanceId)
                ? _botMoveCreator.GenerateDirectionToANearbyPlayerCharacter(instanceId)
                : _botMoveCreator.GenerateDirectionToANearbyPlayerCharacter();

            _charactersTurnOrchestrator.Execute(direction, Team.Bot);
        }

        protected override void OnExit()
        {
            _eventBus.Publish(new TurnEndedEvent(Team.Bot));
            _botPhaseService.SpawnHardBot();
            
            if (Random.value < _botSpawnChancesConfig.SpawnChanceOneCharacter)
            {
                TrySpawnBots(1);
            }

            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any())
                _characterCreator.CreateOnRandomPos(_botSpawnChancesConfig.GetRandomDefaultBot());
        }

        public override void Update() { }
        
        private void TrySpawnBots(int requestedCount)
        {
            var remaining =
                _initialGameplayConfig.MaxCharactersCount -
                _charactersStorage.GetCharactersByTeam(Team.Bot).Count();
            
            var spawnCount = remaining < requestedCount ? remaining : requestedCount;

            if (spawnCount <= 0)
                return;

            _characterCreator.CreateOnRandomPos(_botSpawnChancesConfig.GetRandomDefaultBot());
        }
    }
}
