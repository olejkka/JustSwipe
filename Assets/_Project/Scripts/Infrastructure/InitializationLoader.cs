using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationLoader : MonoBehaviour
{
    private async void Start()
    {
        await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        await SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }
}