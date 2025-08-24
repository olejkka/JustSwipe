using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMover
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly Dictionary<Vector2Int, Character> _claimedPositions = new();
        
        public event Action OnMove;

        
        public CharactersMover(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
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
                var targetPosition = attacker.Position + vector;

                if (_claimedPositions.TryGetValue(targetPosition, out var defender))
                {
                    if (defender.Team != attacker.Team)
                        defender.TakeDamage(attacker.Damage);
                    else
                        attacker.Move(vector);
                }
                else
                    attacker.Move(vector);
            }

            OnMove?.Invoke();
        }
    }
}