using System.Threading.Tasks;
using _Project.Scripts.Infrastructure.Events;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM.States
{
    public class GameplayState : IProjectState
    {
        private readonly SceneLoader _sceneLoader;
        private readonly EventBus _eventBus;
        

        public GameplayState(SceneLoader sceneLoader, EventBus eventBus)
        {
            _sceneLoader = sceneLoader;
            _eventBus = eventBus;
        }

        public async Task Enter()
        {
            await _sceneLoader.LoadGameplay();
            _eventBus.Publish(new StartGameplayEvent());
        }

        public async Task Exit()
        {
            await _sceneLoader.UnloadGameplay();
        }
    }
}