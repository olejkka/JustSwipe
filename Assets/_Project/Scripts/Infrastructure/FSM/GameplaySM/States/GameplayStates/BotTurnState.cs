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
        private readonly BotSpawnChancesConfig _botSpawnChancesConfig;
        private readonly BotMoveCreator _botMoveCreator;
        private readonly CharactersMover _charactersMover;
        private readonly CharacterCreator _characterCreator;
        private readonly CharactersStorage _charactersStorage;
        

        public BotTurnState(
            IReadOnlyList<ITransition> transitions,
            BotSpawnChancesConfig botSpawnChancesConfig,
            BotMoveCreator botMoveCreator,
            CharactersMover charactersMover,
            CharacterCreator characterCreator,
            CharactersStorage charactersStorage
        ) : base(transitions)
        {
            _botSpawnChancesConfig = botSpawnChancesConfig;
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

        protected override void OnExit()
        {
            if (Random.value < _botSpawnChancesConfig.SpawnChanceOneCharacter)
            {
                _characterCreator.CreateOnRandomPos(_botSpawnChancesConfig.GetRandomDefaultBot());
            }
            else if (Random.value < _botSpawnChancesConfig.SpawnChanceTwoCharacters)
            {
                _characterCreator.CreateOnRandomPos(_botSpawnChancesConfig.GetRandomDefaultBot());
                _characterCreator.CreateOnRandomPos(_botSpawnChancesConfig.GetRandomSecondaryBot());
            }
            
            if (!_charactersStorage.GetCharactersByTeam(Team.Bot).Any())
                _characterCreator.CreateOnRandomPos(_botSpawnChancesConfig.GetRandomDefaultBot());
        }

        public override void Update() { }
    }
}