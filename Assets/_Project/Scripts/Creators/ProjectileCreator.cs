using System.Collections.Generic;
using _Project.Scripts.Characters.Movement;
using _Project.Scripts.Characters.Projectiles;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using UnityEngine;

namespace _Project.Scripts.Creators
{
    public class ProjectileCreator
    {
        private readonly EventBus _eventBus;
        private readonly CharactersConfig _charactersConfig;


        public ProjectileCreator(EventBus eventBus, CharactersConfig charactersConfig)
        {
            _eventBus = eventBus;
            _charactersConfig = charactersConfig;
        }

        public void Create(IReadOnlyList<ProjectileShot> shots)
        {
            for (int i = 0; i < shots.Count; i++)
            {
                var shot = shots[i];
                var characterDef = _charactersConfig.GetEntryByDefinitionId(shot.Attacker.DefinitionId);

                if (characterDef == null || string.IsNullOrEmpty(characterDef.ProjectileDefinitionId))
                {
                    Debug.LogError($"No projectile definition id for {shot.Attacker.DefinitionId}");
                    continue;
                }

                var projectile = new Projectile(
                    characterDef.ProjectileDefinitionId,
                    shot.Attacker,
                    shot.Direction,
                    shot.Attacker.Position,
                    shot.ImpactPosition,
                    shot.Target);

                _eventBus.Publish(new ProjectileCreatedEvent(projectile));
            }
        }
    }
}
