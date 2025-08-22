using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.FSM.States.GameplayStates
{
    public class PauseState : State
    {
        public PauseState(IReadOnlyList<ITransition> transitions) : base(transitions)
        {
        }

        public override void Enter()
        {
            Time.timeScale = 0f;
        }

        public override void Exit()
        {
            Time.timeScale = 1f;
        }

        public override void Update()
        {
        }
    }
}