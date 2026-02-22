using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Creators;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates
{
    public class BotTurnState : State
    {
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersStorage _charactersStorage;

        public BotTurnState(
            IReadOnlyList<ITransition> transitions,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            CharacterCreator characterCreator,
            CharactersStorage charactersStorage
        ) : base(transitions)
        {
            _botMoveCreator = botMoveCreator;
            _charactersMover = charactersMover;
            _characterCreator = characterCreator;
            _charactersStorage = charactersStorage;
        }

        protected override void OnEnter()
        {
            var direction = _botMoveCreator.GenerateRandomDirection();
            _charactersMover.Move(direction, Team.Bot);
        }

        public override void Exit()
        {
            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any())
                _characterCreator.CreateOnRandomPos(CharacterType.Bot_2);
        }

        public override void Update() { }
    }
}