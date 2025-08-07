using System;
using _Project.Scripts.Creators;
using _Project.Scripts.InputHandlers;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Characters
{
    public class CharacterSpawnController
    {
        private const float SpawnChance = 0.25f;
        private readonly CharacterCreator _creator;

        
        public CharacterSpawnController(
            CharacterCreator characterCreator
        )
        {
            _creator = characterCreator;
        }

        public void HandleInput(Vector2Int _)
        {
            if (Random.value < SpawnChance)
                _creator.Create(Team.Bot);
        }
    }
}