using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Tiles;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMover
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesPositionsStorage _tilesPositionsStorage;
        private readonly Dictionary<Vector2Int, Character> _claimedPositions = new();
        
        public event Action OnMove;

        
        public CharactersMover(CharactersStorage charactersStorage, TilesPositionsStorage tilesPositionsStorage)
        {
            _charactersStorage = charactersStorage;
            _tilesPositionsStorage = tilesPositionsStorage;
        }

        public void Move(Vector2Int vector, Team team)
        {
            _claimedPositions.Clear();

            var characters = _charactersStorage.GetAllCharacters().ToArray();
            
            for (int i = 0; i < characters.Length; i++)
                _claimedPositions[characters[i].Position] = characters[i];

            var attackers = _charactersStorage.GetCharactersByTeam(team).ToArray();
            
            for (int i = 0; i < attackers.Length; i++)
            {
                var attacker = attackers[i];
                var target = attacker.Position + vector;

                if (_claimedPositions.TryGetValue(target, out var defender))
                    if (defender.Team != attacker.Team)
                        defender.TakeDamage(attacker.Damage);

                attacker.Move(vector);
            }

            KillCharactersOutOfBounds();
            
            OnMove?.Invoke();
        }
        
        private void KillCharactersOutOfBounds()
        {
            var allCharacters = _charactersStorage.GetAllCharacters().ToArray();
            
            for (int i = 0; i < allCharacters.Length; i++)
            {
                var character = allCharacters[i];
                
                if (!_tilesPositionsStorage.Contains(character.Position)) 
                    character.TakeDamage(character.Health);
            }
        }
    }
}