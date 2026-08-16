using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Characters.Storages;
using UnityEngine;

namespace _Project.Scripts.Characters.Movement
{
    public class CharactersMover
    {
        private readonly CharactersStorage _charactersStorage;

        
        public CharactersMover(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public IReadOnlyList<MeleeCollision> Move(Vector2Int vector, Team team)
        {
            var claimedPositions = new Dictionary<Vector2Int, Character>();
            var collisions = new List<MeleeCollision>();
            var characters = _charactersStorage.GetAllCharacters().ToArray();

            for (int i = 0; i < characters.Length; i++)
                claimedPositions[characters[i].Position] = characters[i];

            var attackers = _charactersStorage.GetCharactersByTeam(team).ToArray();

            for (int i = 0; i < attackers.Length; i++)
            {
                var attacker = attackers[i];
                var target = attacker.Position + vector;

                if (claimedPositions.TryGetValue(target, out var occupant))
                {
                    if (occupant.Team != attacker.Team)
                        collisions.Add(new MeleeCollision(attacker, occupant));

                    continue;
                }

                attacker.Move(vector);
            }

            return collisions;
        }
    }
}