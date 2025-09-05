using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases
{
    public class CharactersMovingPhase : Phase
    {
        private readonly CharactersMovementHandler _charactersMovementHandler;
        private readonly InputStorage _inputStorage;
        private Vector2Int[] _dirs =
        {
            Vector2Int.up, 
            Vector2Int.down, 
            Vector2Int.left, 
            Vector2Int.right
        };
        

        public CharactersMovingPhase(
            CharactersMovementHandler charactersMovementHandler,
            InputStorage inputStorage
            )
        {
            _charactersMovementHandler = charactersMovementHandler;
            _inputStorage = inputStorage;
        }
        
        public override void Enter()
        {
            if (_humanPhase)
                _charactersMovementHandler.ProcessMovementForTeam(_inputStorage.InputVector, Team.Player);
            else
                _charactersMovementHandler.ProcessMovementForTeam(_dirs[Random.Range(0, _dirs.Length)], Team.Bot);
            
            _humanPhase = !_humanPhase;
            Exit();
        }
    }
}