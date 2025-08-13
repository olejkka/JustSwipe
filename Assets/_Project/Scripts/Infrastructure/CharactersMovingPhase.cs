using _Project.Scripts.Characters;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.Infrastructure
{
    public class CharactersMovingPhase : Phase
    {
        private readonly CharactersMover _charactersMover;
        private readonly InputStorage _inputStorage;
        private Vector2Int[] _dirs =
        {
            Vector2Int.up, 
            Vector2Int.down, 
            Vector2Int.left, 
            Vector2Int.right
        };
        

        public CharactersMovingPhase(
            CharactersMover charactersMover,
            InputStorage inputStorage
            )
        {
            _charactersMover = charactersMover;
            _inputStorage = inputStorage;
        }
        
        public override void Enter()
        {
            Debug.Log("Entering CharactersMovingPhase");
            
            if (_humanPhase)
                _charactersMover.Move(_inputStorage.InputVector, Team.Player);
            else
                _charactersMover.Move(_dirs[Random.Range(0, _dirs.Length)], Team.Bot);
            
            _humanPhase = !_humanPhase;
            Exit();
        }
    }
}