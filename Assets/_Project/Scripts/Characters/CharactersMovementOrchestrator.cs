using System;
using UnityEngine;

namespace _Project.Scripts.Characters
{
    public class CharactersMovementOrchestrator
    {
        private readonly CharactersCombatHandler _combatHandler;
        private readonly CharactersMovementHandler _movementHandler;
        private readonly CharactersPositionValidator _positionValidator;
        
        public event Action OnTurnCompleted;

        public CharactersMovementOrchestrator(
            CharactersCombatHandler combatHandler,
            CharactersMovementHandler movementHandler,
            CharactersPositionValidator positionValidator)
        {
            _combatHandler = combatHandler;
            _movementHandler = movementHandler;
            _positionValidator = positionValidator;
        }

        public void ProcessTurn(Vector2Int direction, Team team)
        {
            _combatHandler.ProcessCombatForTeam(direction, team);
            _movementHandler.ProcessMovementForTeam(direction, team);
            _positionValidator.ValidateAllCharacters();
            
            OnTurnCompleted?.Invoke();
        }
    }
}