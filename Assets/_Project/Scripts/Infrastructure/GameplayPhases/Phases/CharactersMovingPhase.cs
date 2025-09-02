using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.GameplayPhases.Phases
{
    public class CharactersMovingPhase : Phase, IOrderedPhase
    {
        private readonly CharactersMovementOrchestrator _charactersMovementOrchestrator;
        private readonly InputStorage _inputStorage;
        private Vector2Int[] _dirs =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        
        public int Order => 2;
        

        public CharactersMovingPhase(
            CharactersMovementOrchestrator charactersMovementOrchestrator,
            InputStorage inputStorage
            )
        {
            _charactersMovementOrchestrator = charactersMovementOrchestrator;
            _inputStorage = inputStorage;
        }
        
        public override void Enter()
        {
            if (_humanPhase)
                _charactersMovementOrchestrator.ProcessTurn(_inputStorage.InputVector, Team.Player);
            else
                _charactersMovementOrchestrator.ProcessTurn(_dirs[Random.Range(0, _dirs.Length)], Team.Bot);
            
            _humanPhase = !_humanPhase;
            
            Exit();
        }
    }
}