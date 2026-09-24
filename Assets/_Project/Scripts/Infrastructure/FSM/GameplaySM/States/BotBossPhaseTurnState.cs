using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Movement;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.FSM.Core;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States
{
    public class BotBossPhaseTurnState : State
    {
        private readonly EventBus.EventBus _eventBus;
        private readonly BotPhaseCharactersConfig _botPhaseCharactersConfig;
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersTurnOrchestrator _charactersTurnOrchestrator;
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersStorage _charactersStorage;
        private readonly BotPhaseService _botPhaseService;


        public BotBossPhaseTurnState(
            IReadOnlyList<ITransition> transitions,
            EventBus.EventBus eventBus,
            BotPhaseCharactersConfig botPhaseCharactersConfig,
            BotMoveCreator botMoveCreator,
            CharactersTurnOrchestrator charactersTurnOrchestrator,
            CharacterCreator characterCreator,
            CharactersStorage charactersStorage,
            BotPhaseService botPhaseService) : base(transitions)
        {
            _eventBus = eventBus;
            _botPhaseCharactersConfig = botPhaseCharactersConfig;
            _botMoveCreator = botMoveCreator;
            _charactersTurnOrchestrator = charactersTurnOrchestrator;
            _characterCreator = characterCreator;
            _charactersStorage = charactersStorage;
            _botPhaseService = botPhaseService;
        }

        protected override void OnEnter()
        {
            var direction = _botPhaseService.TryGetBossInstanceId(out var instanceId)
                ? _botMoveCreator.GenerateDirectionToANearbyPlayerCharacter(instanceId)
                : _botMoveCreator.GenerateDirectionToANearbyPlayerCharacter();

            _charactersTurnOrchestrator.Execute(direction, Team.Bot);
        }

        protected override void OnExit()
        {
            _eventBus.Publish(new TurnEndedEvent(Team.Bot));
            _botPhaseService.SpawnBoss();

            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any())
                _characterCreator.CreateOnRandomPos(_botPhaseCharactersConfig.GetRandomDefaultBot());
        }

        public override void Update() { }
    }
}
