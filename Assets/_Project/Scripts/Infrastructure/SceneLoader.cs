using _Project.Scripts.Infrastructure.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace _Project.Scripts.Infrastructure
{
    public class SceneLoader : MonoBehaviour
    {
        [Inject] private EventBus _eventBus;
        
        
        private async void Start()
        {
            await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync("GameplayUI", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gameplay"));
        }
    }
}