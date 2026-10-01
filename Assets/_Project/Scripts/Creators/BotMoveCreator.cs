using System.Linq;
using _Project.Scripts.Board;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using UnityEngine;

namespace _Project.Scripts.Creators
{
    public class BotMoveCreator
    {
        private readonly CharactersStorage _charactersStorage;
        private readonly TilesStorage _tilesStorage;
        
        private readonly Vector2Int[] _directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        

        public BotMoveCreator(CharactersStorage charactersStorage, TilesStorage tilesStorage)
        {
            _charactersStorage = charactersStorage;
            _tilesStorage = tilesStorage;
        }

        public Vector2Int GenerateDirectionToANearbyPlayerCharacter()
        {
            var playerCharacters = _charactersStorage.GetCharactersByTeam(Team.Player);
            var botCharacters = _charactersStorage.GetCharactersByTeam(Team.Bot);
            
            var bestDistance = int.MaxValue;
            var bestDelta = Vector2Int.zero;
            
            Character origin = null;
            
            foreach (var botCharacter in botCharacters)
            {
                foreach (var playerCharacter in playerCharacters)
                {
                    var delta = playerCharacter.Position - botCharacter.Position;
                    var distance = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
                    
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestDelta = delta;
                        origin = botCharacter;
                    }
                }
            }

            if (origin == null)
                return DirectionFromDelta(bestDelta);

            return PickDirection(origin, bestDelta);
        }

        public Vector2Int GenerateDirectionToANearbyPlayerCharacter(int instanceId)
        {
            var origin = _charactersStorage.GetAllCharacters().First(character => character.InstanceId == instanceId);
            var playerCharacters = _charactersStorage.GetCharactersByTeam(Team.Player);

            var bestDistance = int.MaxValue;
            var bestDelta = Vector2Int.zero;

            foreach (var playerCharacter in playerCharacters)
            {
                var delta = playerCharacter.Position - origin.Position;
                var distance = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestDelta = delta;
                }
            }

            return PickDirection(origin, bestDelta);
        }
        
        public Vector2Int GenerateRandomDirection() => 
            _directions[Random.Range(0, _directions.Length)];

        private Vector2Int PickDirection(Character origin, Vector2Int delta)
        {
            var preferred = DirectionFromDelta(delta);

            if (origin.IsRanged)
                return preferred;

            if (IsWalkableTile(origin.Position + preferred))
                return preferred;

            for (int i = 0; i < _directions.Length; i++)
            {
                var direction = _directions[i];

                if (direction == preferred)
                    continue;

                if (IsWalkableTile(origin.Position + direction))
                    return direction;
            }

            return GenerateRandomDirection();
        }

        private bool IsWalkableTile(Vector2Int position) =>
            _tilesStorage.TryGet(position, out var tile) && tile.IsWalkable;

        private static Vector2Int DirectionFromDelta(Vector2Int delta)
        {
            var absX = Mathf.Abs(delta.x);
            var absY = Mathf.Abs(delta.y);

            if (absX > absY)
                return new Vector2Int(delta.x > 0 ? 1 : -1, 0);

            if (absY > absX)
                return new Vector2Int(0, delta.y > 0 ? 1 : -1);

            if (Random.value < 0.5f)
                return new Vector2Int(delta.x > 0 ? 1 : -1, 0);

            return new Vector2Int(0, delta.y > 0 ? 1 : -1);
        }
    }
}
