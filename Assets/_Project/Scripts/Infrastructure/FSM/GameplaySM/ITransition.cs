using System;

namespace _Project.Scripts.Infrastructure.FSM.GameplaySM
{
    public interface ITransition
    {
        public Type NextState { get;}

        public bool CanTransit();
        
        public void Reset();
    }
}