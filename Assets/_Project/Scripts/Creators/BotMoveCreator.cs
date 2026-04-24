using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Characters.Structs;
using UnityEngine;

namespace _Project.Scripts.Creators
{
    public class BotMoveCreator
    {
        private readonly CharactersStorage _charactersStorage;
        
        private readonly Vector2Int[] _directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        

        public BotMoveCreator(CharactersStorage charactersStorage)
        {
            _charactersStorage = charactersStorage;
        }

        public Vector2Int GenerateDirectionToANearbyPlayerCharacter()
        {
            var playerCharacters = _charactersStorage.GetCharactersByTeam(Team.Player);
            var botCharacters = _charactersStorage.GetCharactersByTeam(Team.Bot);
            
            var bestDistance = int.MaxValue;
            var bestDelta = Vector2Int.zero;
            
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
                    }
                }
            }

            var absX = Mathf.Abs(bestDelta.x);
            var absY = Mathf.Abs(bestDelta.y);
            
            if (absX > absY)
                return new Vector2Int(bestDelta.x > 0 ? 1 : -1, 0);
            
            if (absY > absX)
                return new Vector2Int(0, bestDelta.y > 0 ? 1 : -1);
            
            if (Random.value < 0.5f)
                return new Vector2Int(bestDelta.x > 0 ? 1 : -1, 0);
            
            return new Vector2Int(0, bestDelta.y > 0 ? 1 : -1);
        }
        
        public Vector2Int GenerateRandomDirection() => 
            _directions[Random.Range(0, _directions.Length)];
    }
}