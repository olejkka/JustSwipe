using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Configs;
using _Project.Scripts.Creators;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM.States.GameplayStates
{
    public class BotTurnState : State
    {
        private readonly CharacterSpawnChancesConfig _characterSpawnChancesConfig;
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
            var direction = _botMoveCreator.GenerateDirectionToANearbyPlayerCharacter();
            _charactersMover.Move(direction, Team.Bot);
        }

        public override void Exit()
        {
            if (Random.value < _characterSpawnChancesConfig.SpawnChanceOneCharacter)
            {
                _characterCreator.CreateOnRandomPos(CharacterType.Bot_1);
            }
            else if (Random.value < _characterSpawnChancesConfig.SpawnChanceTwoCharacter)
            {
                _characterCreator.CreateOnRandomPos(CharacterType.Bot_1);
                _characterCreator.CreateOnRandomPos(CharacterType.Bot_2);
            }
            
            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any())
                _characterCreator.CreateOnRandomPos(CharacterType.Bot_2);
        }

        public override void Update() { }
    }
}