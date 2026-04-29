using System;
using System.Collections.Generic;
using _Project.Scripts.Infrastructure.FSM.ProjectSM.States;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM
{
    public class ProjectStatesProvider
    {
        private readonly SceneLoader _sceneLoader;
        private readonly EventBus.EventBus _eventBus;
        
        
        public ProjectStatesProvider(SceneLoader sceneLoader,  EventBus.EventBus eventBus)
        {
            _sceneLoader = sceneLoader;
            _eventBus = eventBus;
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
                    new MenuState(_sceneLoader,  _eventBus)
                },
                {
                    typeof(GameplayState), 
                    new GameplayState(_sceneLoader,  _eventBus)
                },
            };
        }
    }
}