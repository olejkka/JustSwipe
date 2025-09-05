using System;

namespace _Project.Scripts.Infrastructure.GameplayPhases.Phases
{
    public abstract class Phase : IPhase
    {
        public event Action OnExit;

        public abstract void Enter();

        protected void Exit()
        {
            OnExit?.Invoke();
        }
    }

}