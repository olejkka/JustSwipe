using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Infrastructure
{
    public class SceneLoader : MonoBehaviour
    {
        private async void Start()
        {
            await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        }
    }
}