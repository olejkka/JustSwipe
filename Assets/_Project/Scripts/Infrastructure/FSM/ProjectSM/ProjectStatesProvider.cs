using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.ProjectSM.States;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM
{
    public class ProjectStatesProvider
    {
        private readonly SceneLoader _sceneLoader;

        public ProjectStatesProvider(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public Dictionary<Type, IProjectState> CreateStates()
        {
            return new Dictionary<Type, IProjectState>
            {
                {
                    typeof(InitializationState), 
                    new InitializationState()
                },
                {
                    typeof(MenuState), 
                    new MenuState(_sceneLoader)
                },
                {
                    typeof(GameplayState), 
                    new GameplayState(_sceneLoader)
                },
            };
        }
    }
}