using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure
{
    public class SceneLoader
    {
        public async Task LoadMenu()
        {
            await SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Additive);
        }

        public async Task UnloadMenu()
        {
            await SceneManager.UnloadSceneAsync("Menu");
        }

        public async Task LoadGameplay()
        {
            await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync("GameplayUI", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gameplay"));
        }

        public async Task UnloadGameplay()
        {
            await SceneManager.UnloadSceneAsync("GameplayUI");
            await SceneManager.UnloadSceneAsync("Gameplay");
        }
    }
}