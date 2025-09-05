using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMovementHandler
    {
        private readonly CharactersStorage _charactersStorage;

        public CharactersMovementHandler(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public void ProcessMovementForTeam(Vector2Int direction, Team team)
        {
            var characters = _charactersStorage.GetCharactersByTeam(team).ToArray();
            var occupiedPositions = GetOccupiedPositions();
            
            foreach (var character in characters)
            {
                var targetPosition = character.Position + direction;
                
                if (CanMoveToPosition(targetPosition, occupiedPositions, character.CharacterConfig.Team))
                {
                    character.Move(direction);
                }
            }
        }

        private bool CanMoveToPosition(Vector2Int position, Dictionary<Vector2Int, Character> occupiedPositions, Team characterTeam)
        {
            if (!occupiedPositions.ContainsKey(position))
                return true;
            
            var characterAtPosition = occupiedPositions[position];
            return characterAtPosition.CharacterConfig.Team == characterTeam;
        }

        private Dictionary<Vector2Int, Character> GetOccupiedPositions()
        {
            var occupiedPositions = new Dictionary<Vector2Int, Character>();
            var allCharacters = _charactersStorage.GetAllCharacters();
            
            foreach (var character in allCharacters)
            {
                occupiedPositions[character.Position] = character;
            }
            
            return occupiedPositions;
        }
    }
}