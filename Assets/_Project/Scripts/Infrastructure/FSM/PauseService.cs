using System;

namespace _Project.Scripts.Infrastructure.FSM
{
    public class PauseService
    {
        public bool IsPaused { get; private set; }
        public bool ResumeToPlayer { get; set; }

        
        public void RequestPause()
        {
            if (IsPaused)
                return;
            
            IsPaused = true;
        }

        public void RequestResume()
        {
            if (!IsPaused)
                return;
            
            IsPaused = false;
        }
    }
}