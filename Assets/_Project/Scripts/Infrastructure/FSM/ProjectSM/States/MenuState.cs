using System.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM.States
{
    public class MenuState : IProjectState
    {
        private readonly SceneLoader _sceneLoader;

        public MenuState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public async Task Enter()
        {
            await _sceneLoader.LoadMenu();
        }

        public async Task Exit()
        {
            await _sceneLoader.UnloadMenu();
        }
    }
}