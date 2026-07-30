using _Project.Scripts.Characters.StructsEnums;

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