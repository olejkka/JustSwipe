using System;
using UnityEngine;

namespace _Project.Scripts.Characters.Projectiles
{
    [Serializable]
    public class ProjectileAnimationData
    {
        public float FrameRate = 8f;
        public Sprite[] Projectile;
        public float Speed = 8f;
    }
}
