using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersCombatHandler
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly Dictionary<Character, Character> _lastAttackers = new();

        public CharactersCombatHandler(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public void ProcessCombatForTeam(Vector2Int direction, Team team)
        {
            var attackers = _charactersStorage.GetCharactersByTeam(team).ToArray();
            var defenders = GetDefendersAtTargetPositions(attackers, direction);
            
            foreach (var attacker in attackers)
            {
                var targetPosition = attacker.Position + direction;
                
                if (defenders.TryGetValue(targetPosition, out var defender))
                    if (defender.CharacterConfig.Team != attacker.CharacterConfig.Team)
                        ProcessAttack(attacker, defender);
            }
        }

        private void ProcessAttack(Character attacker, Character defender)
        {
            _lastAttackers.Clear();
            _lastAttackers[defender] = attacker;
            defender.TakeDamage(attacker._stats.Attack);
        }
        
        private Dictionary<Vector2Int, Character> GetDefendersAtTargetPositions(IEnumerable<Character> attackers, Vector2Int direction)
        {
            var targetPositions = attackers.Select(a => a.Position + direction).ToHashSet();
            var allCharacters = _charactersStorage.GetAllCharacters();
            
            return allCharacters
                .Where(c => targetPositions.Contains(c.Position))
                .ToDictionary(c => c.Position);
        }
        
        public Character GetLastAttacker(Character character)
        {
            _lastAttackers.TryGetValue(character, out var attacker);
            return attacker;
        }
    }
}