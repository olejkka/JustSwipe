#if UNITY_EDITOR
using System;
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

    // Меню для переключения maximize у активного окна
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

    // Глобальный шорткат F
    [Shortcut("JustSwipe/Toggle Active Window Maximize", KeyCode.F)]
    // И специфично для SceneView, чтобы переопределить Frame Selected
    [Shortcut("JustSwipe/Toggle Active Window Maximize (SceneView)", typeof(SceneView), KeyCode.F)]
    private static void Shortcut_ToggleActiveWindowMaximize()
    {
        ToggleActiveWindowMaximize();
    }
#endif

    private static void ToggleActiveWindowMaximize()
    {
        // Текущее выбранное окно (или окно под курсором, если фокус отсутствует)
        EditorWindow window = EditorWindow.focusedWindow ?? EditorWindow.mouseOverWindow;
        if (window == null)
            return;

        ToggleMaximized(window);
    }

    private static void ToggleMaximized(EditorWindow window)
    {
        if (window == null)
            return;

        window.Focus();                // активируем вкладку
        window.maximized = !window.maximized; // эффект двойного клика по вкладке
    }
}
#endif