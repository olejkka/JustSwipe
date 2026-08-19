using System;
using UnityEngine;

namespace _Project.Scripts.Characters.Projectiles
{
    [Serializable]
    public class ProjectileDefinition
    {
        [Header("Identity")]
        [SerializeField] private string _definitionId;

        [Header("Presentation")]
        [SerializeField] private ProjectileAnimationData _animations;

        public string DefinitionId => _definitionId;
        public ProjectileAnimationData Animations => _animations;
    }
}
