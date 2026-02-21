using System;

namespace _Project.Scripts.Infrastructure.FSM.States
{
    public interface IState
    {
        public void Enter();

        public void Exit();

        public void Update();

        public bool TryGetNextState(out Type type);
    }
}