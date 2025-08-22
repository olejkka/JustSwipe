using _Project.Scripts.Creators;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Characters
{
    public class EnemyCharacterSpawnController
    {
        private const float SpawnChance = 0.25f;
        private readonly CharacterCreator _creator;


        public EnemyCharacterSpawnController(CharacterCreator characterCreator)
        {
            _creator = characterCreator;
        }

        public void HandlePlayerInput(Vector2Int vector, Team team)
        {
            if (Random.value < SpawnChance)
                _creator.Create("Bot_1");
        }
    }
}