using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private async void Start()
    {
        await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        await SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }
}