using System.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM.States
{
    public class GameplayState : IProjectState
    {
        private readonly SceneLoader _sceneLoader;

        public GameplayState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async Task Enter()
        {
            await _sceneLoader.LoadGameplay();
        }

        public async Task Exit()
        {
            await _sceneLoader.UnloadGameplay();
        }
    }
}