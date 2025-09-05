using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private async void Start()
    {
        await SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Gameplay"));
        await SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
    }
}