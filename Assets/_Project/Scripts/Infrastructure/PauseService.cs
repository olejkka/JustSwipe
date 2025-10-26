using System;
using UnityEngine;

public class PauseService
{
    private bool _isPaused;
    
    public bool IsPaused => _isPaused;
    
    public event Action OnPaused;
    public event Action OnResumed;
    

    public void TogglePause()
    {
        if (_isPaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        if (_isPaused) 
            return;
        
        _isPaused = true;
        Time.timeScale = 0f;
        OnPaused?.Invoke();
    }

    private void Resume()
    {
        if (!_isPaused) 
            return;
        
        _isPaused = false;
        Time.timeScale = 1f;
        OnResumed?.Invoke();
    }
}