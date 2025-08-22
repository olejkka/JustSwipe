using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.InputHandlers
{
    public class PlayerInputHandler
    {
        private const float SpawnChance = 0.25f;
        
        private readonly CharacterCreator _creator;
        private readonly CharacterStatsConfig _characterStatsConfig;
        

        public PlayerInputHandler(
            CharacterCreator characterCreator,
            CharacterStatsConfig characterStatsConfig
            )
        {
            _creator = characterCreator;
            _characterStatsConfig = characterStatsConfig;
        }
        
        public void Handle(Vector2Int vector, Team team)
        {
            string randomBot = _characterStatsConfig.GetRandomCharacterIdByTeam(Team.Bot);
            
            if (Random.value < SpawnChance)
                _creator.Create(randomBot);
        }
    }
}