using System.Threading.Tasks;
using _Project.Scripts.Infrastructure.EventBus.Events;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM.States
{
    public class MenuState : IProjectState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly EventBus.EventBus _eventBus;

        public MenuState(SceneLoader sceneLoader, EventBus.EventBus eventBus)
        {
            _sceneLoader = sceneLoader;
            _eventBus = eventBus;
        }

        public async Task Enter()
        {
            await _sceneLoader.LoadMenu();
            _eventBus.Publish(new MenuEnteredEvent());
        }

        public async Task Exit()
        {
            await _sceneLoader.UnloadMenu();
        }
    }
}