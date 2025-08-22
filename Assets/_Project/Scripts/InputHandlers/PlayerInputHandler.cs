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


        public PlayerInputHandler(CharacterCreator characterCreator)
        {
            _creator = characterCreator;
        }

        public void Handle(Vector2Int vector, Team team)
        {
            if (Random.value < SpawnChance)
                _creator.Create("Bot_1");
        }
    }
}