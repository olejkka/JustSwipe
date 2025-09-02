using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.InputHandlers
{
    public class PlayerInputHandler
    {
        private const float SpawnChance = 0.25f;
        
        private readonly CharacterCreator _creator;
        
        private readonly IReadOnlyList<CharacterConfig> _botCharacterConfigs;
        

        public PlayerInputHandler(
            CharacterCreator characterCreator,
            IReadOnlyList<CharacterConfig> characterConfigs
            )
        {
            _creator = characterCreator;
            _botCharacterConfigs = characterConfigs.Where(config => config.Team == Team.Bot).ToList();
        }
        
        public void Handle(Vector2Int vector, Team team)
        {
            if (Random.value > SpawnChance)
                return;

            _creator.Create(_botCharacterConfigs[Random.Range(0, _botCharacterConfigs.Count)]);
        }
    }
}