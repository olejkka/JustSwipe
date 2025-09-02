using System;

namespace _Project.Scripts.Infrastructure.GameplayPhases
{
    public abstract class Phase : IPhase
    {
        public event Action OnExit;
        
        protected bool _humanPhase = true;

        
        public abstract void Enter();

        protected void Exit() => OnExit?.Invoke();
    }
}