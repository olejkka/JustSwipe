using _Project.Scripts.Characters.Health;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;

namespace _Project.Scripts.Characters.Movement
{
    public class CharactersMovementOrchestrator
    {
        private readonly CharactersMover _mover;
        private readonly CharactersPositionResolver _positionResolver;
        private readonly CharactersMeleeResolver _meleeResolver;
        private readonly HealthChangeService _healthChangeService;
        private readonly EventBus _eventBus;

        
        public CharactersMovementOrchestrator(
            CharactersMover mover,
            CharactersPositionResolver positionResolver,
            CharactersMeleeResolver meleeResolver,
            HealthChangeService healthChangeService,
            EventBus eventBus)
        {
            _mover = mover;
            _positionResolver = positionResolver;
            _meleeResolver = meleeResolver;
            _healthChangeService = healthChangeService;
            _eventBus = eventBus;
        }

        public void ExecuteMovement(Vector2Int vector, Team team)
        {
            var collisions = _mover.Move(vector, team);
            _positionResolver.Resolve();
            _meleeResolver.Resolve(collisions);
            _healthChangeService.Apply();

            if (team == Team.Player)
                _eventBus.Publish(new PlayerMoveCompletedEvent());
            else
                _eventBus.Publish(new BotMoveCompletedEvent());
        }
    }
}