using _Project.Scripts.Characters.Structs;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class TurnEndedEvent
    {
        public Team Team { get; }
        
        
        public TurnEndedEvent(Team team)
        {
            Team = team;
        }
    }
}