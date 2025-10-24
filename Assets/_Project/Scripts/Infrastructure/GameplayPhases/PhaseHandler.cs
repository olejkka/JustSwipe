using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure
{
    public class PhaseHandler : IStartable
    {
        private readonly IReadOnlyList<Phase> _phases;
        private int _currentPhase = -1;

        
        public PhaseHandler(IReadOnlyList<Phase> phases)
        {
            _phases = phases;
        }


        public void Start()
        {
            // if (_phases == null || _phases.Count == 0)
            // {
            //     Debug.LogWarning("PhaseHandler: no phases registered");
            //     return;
            // }
            //
            // EnterNextPhase();
        }

        private void EnterNextPhase()
        {
            if (_currentPhase >= 0)
                _phases[_currentPhase].OnExit -= EnterNextPhase;

            _currentPhase = (_currentPhase + 1) % _phases.Count;

            _phases[_currentPhase].OnExit += EnterNextPhase;
            _phases[_currentPhase].Enter();
        }
    }
}