using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Board;
using _Project.Scripts.Characters.Health;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.Characters.Movement
{
    public class ProjectilesResolver
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesStorage _tilesStorage;
        private readonly HealthChangeService _healthChangeService;
        private readonly ProjectilesConfig _projectilesConfig;


        public ProjectilesResolver(
            CharactersStorage charactersStorage,
            TilesStorage tilesStorage,
            HealthChangeService healthChangeService,
            ProjectilesConfig projectilesConfig)
        {
            _charactersStorage = charactersStorage;
            _tilesStorage = tilesStorage;
            _healthChangeService = healthChangeService;
            _projectilesConfig = projectilesConfig;
        }

        public IReadOnlyList<ProjectileShot> Process(Vector2Int vector, Team team)
        {
            var shots = new List<ProjectileShot>();

            if (vector == Vector2Int.zero)
                return shots;

            if (!_tilesStorage.TryGetBounds(out var min, out var max))
                return shots;

            var occupants = new Dictionary<Vector2Int, Character>();
            var characters = _charactersStorage.GetAllCharacters().ToArray();

            for (int i = 0; i < characters.Length; i++)
                occupants[characters[i].Position] = characters[i];

            var attackers = _charactersStorage.GetCharactersByTeam(team).ToArray();

            for (int i = 0; i < attackers.Length; i++)
            {
                var attacker = attackers[i];

                if (!attacker.IsRanged)
                    continue;

                var shot = Trace(attacker, vector, min, max, occupants);
                shots.Add(shot);

                if (shot.Target == null)
                    continue;

                _healthChangeService.Enqueue(
                    HealthChangeRequest.Damage(
                        HealthChangeSource.FromCharacter(attacker),
                        shot.Target,
                        attacker.TotalDamage));
            }

            return shots;
        }

        private ProjectileShot Trace(
            Character attacker,
            Vector2Int vector,
            Vector2Int min,
            Vector2Int max,
            IReadOnlyDictionary<Vector2Int, Character> occupants)
        {
            var pos = attacker.Position + vector;

            while (IsInsideBounds(pos, min, max))
            {
                if (occupants.TryGetValue(pos, out var occupant))
                {
                    var distance = Mathf.Abs(pos.x - attacker.Position.x) + Mathf.Abs(pos.y - attacker.Position.y);

                    if (distance <= attacker.AttackRange)
                        return new ProjectileShot(attacker, vector, occupant, occupant.Position);

                    break;
                }

                pos += vector;
            }

            return new ProjectileShot(attacker, vector, null, GetMissEnd(attacker.Position, vector, min, max));
        }

        private Vector2Int GetMissEnd(Vector2Int start, Vector2Int vector, Vector2Int min, Vector2Int max)
        {
            var pos = start + vector;

            while (IsInsideBounds(pos, min, max))
                pos += vector;

            var size = vector.x != 0 ? max.x - min.x + 1 : max.y - min.y + 1;
            return pos + vector * (size * _projectilesConfig.MissTravelBoardSizeMultiplier);
        }

        private static bool IsInsideBounds(Vector2Int pos, Vector2Int min, Vector2Int max) =>
            pos.x >= min.x && pos.x <= max.x && pos.y >= min.y && pos.y <= max.y;
    }
}
