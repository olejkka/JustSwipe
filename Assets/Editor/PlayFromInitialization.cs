#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class PlayFromInitialization
{
    private const string InitSceneName = "Initialization";

    static PlayFromInitialization()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingEditMode)
        {
            var sceneAsset = FindInitializationScene();
            if (sceneAsset != null)
            {
                EditorSceneManager.playModeStartScene = sceneAsset;
            }
            else
            {
                Debug.LogWarning($"Scene '{InitSceneName}' не найдена. Запуск с текущей сцены.");
            }
        }
        else if (change == PlayModeStateChange.EnteredEditMode)
        {
            EditorSceneManager.playModeStartScene = null;
        }
    }

    private static SceneAsset FindInitializationScene()
    {
        var guids = AssetDatabase.FindAssets($"t:Scene {InitSceneName}");
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (Path.GetFileNameWithoutExtension(path) == InitSceneName)
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        }
        return null;
    }
}
#endif