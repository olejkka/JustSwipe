using System;

namespace _Project.Scripts.Infrastructure
{
    public abstract class Phase : IPhase
    {
        protected bool _humanPhase = true; 
        public event Action OnExit;

        public abstract void Enter();

        protected void Exit()
        {
            OnExit?.Invoke();
        }
    }

    public interface IPhase
    {
    }
}