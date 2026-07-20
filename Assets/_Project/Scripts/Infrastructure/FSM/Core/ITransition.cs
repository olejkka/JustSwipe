using System;

namespace _Project.Scripts.Infrastructure.FSM.Core
{
    public interface ITransition
    {
        public Type NextState { get;}
        public bool CanTransit();
        public void Reset();
        void Activate();
        void Deactivate();
    }
}