using _Project.Scripts.Characters.Health;
using _Project.Scripts.Creators;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;

namespace _Project.Scripts.Characters.Movement
{
    public class CharactersTurnOrchestrator
    {
        private readonly CharactersMover _mover;
        private readonly CharactersPositionResolver _positionResolver;
        private readonly CharactersMeleeResolver _meleeResolver;
        private readonly ProjectilesResolver _projectilesResolver;
        private readonly ProjectileCreator _projectileCreator;
        private readonly HealthChangeService _healthChangeService;
        private readonly EventBus _eventBus;


        public CharactersTurnOrchestrator(
            CharactersMover mover,
            CharactersPositionResolver positionResolver,
            CharactersMeleeResolver meleeResolver,
            ProjectilesResolver projectilesResolver,
            ProjectileCreator projectileCreator,
            HealthChangeService healthChangeService,
            EventBus eventBus)
        {
            _mover = mover;
            _positionResolver = positionResolver;
            _meleeResolver = meleeResolver;
            _projectilesResolver = projectilesResolver;
            _projectileCreator = projectileCreator;
            _healthChangeService = healthChangeService;
            _eventBus = eventBus;
        }

        public void Execute(Vector2Int vector, Team team)
        {
            var collisions = _mover.Move(vector, team);
            _positionResolver.Resolve();
            _meleeResolver.Resolve(collisions);
            
            var shots = _projectilesResolver.Process(vector, team);
            _healthChangeService.Apply();
            _projectileCreator.Create(shots);

            if (team == Team.Player)
                _eventBus.Publish(new PlayerMoveCompletedEvent());
            else
                _eventBus.Publish(new BotMoveCompletedEvent());
        }
    }
}
