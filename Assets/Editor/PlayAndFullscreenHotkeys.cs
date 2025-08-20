#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#if UNITY_2019_1_OR_NEWER
using UnityEditor.ShortcutManagement;
#endif

public static class PlayAndFullscreenHotkeys
{
    [MenuItem("Tools/Play _p")]
    private static void TogglePlayMode()
    {
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    [MenuItem("Tools/Pause _#p")]
    private static void TogglePause()
    {
        if (EditorApplication.isPlaying)
            EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    [MenuItem("Tools/Toggle Active Window Maximize _f")]
    private static void ToggleActiveWindowMaximize_Menu()
    {
        ToggleActiveWindowMaximize();
    }

#if UNITY_2019_1_OR_NEWER
    [Shortcut("JustSwipe/Play", KeyCode.P)]
    private static void Shortcut_Play()
    {
        TogglePlayMode();
    }

    [Shortcut("JustSwipe/Pause", KeyCode.P, ShortcutModifiers.Shift)]
    private static void Shortcut_Pause()
    {
        TogglePause();
    }

    [Shortcut("JustSwipe/Toggle Active Window Maximize", KeyCode.F)]
    [Shortcut("JustSwipe/Toggle Active Window Maximize (SceneView)", typeof(SceneView), KeyCode.F)]
    private static void Shortcut_ToggleActiveWindowMaximize()
    {
        ToggleActiveWindowMaximize();
    }
#endif

    private static void ToggleActiveWindowMaximize()
    {
        EditorWindow window = EditorWindow.mouseOverWindow ?? EditorWindow.focusedWindow;
        if (window == null)
            return;

        ToggleMaximized(window);
    }

    private static void ToggleMaximized(EditorWindow window)
    {
        if (window == null)
            return;

        window.Focus();
        window.maximized = !window.maximized;
    }
}
#endif
