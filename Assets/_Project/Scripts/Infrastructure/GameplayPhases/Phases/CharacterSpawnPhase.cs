using _Project.Scripts.Characters;
using _Project.Scripts.Creators;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases.Phases
{
    public class CharactersSpawnPhase : Phase
    {
        private const float SpawnChance = 0.25f;
        
        private readonly CharacterCreator _creator;
        private readonly CharacterStatsConfig _characterStatsConfig;
        

        public CharactersSpawnPhase(
            CharacterCreator creator,
            CharacterStatsConfig characterStatsConfig
        )
        {
            _creator = creator;
            _characterStatsConfig = characterStatsConfig;
        }
        
        public override void Enter()
        {
            string randomBot = _characterStatsConfig.GetRandomCharacterIdByTeam(Team.Bot);
            
            if (Random.value < SpawnChance)
                _creator.Create(randomBot);
            
            Exit();
        }
    }
}