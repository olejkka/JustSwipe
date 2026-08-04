using _Project.Scripts.Characters;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class ApplyEffectEvent
    {
        public Team Team { get; }
        public string DefinitionId { get; }
        
        
        public ApplyEffectEvent(Team team, string definitionId)
        {
            Team = team;
            DefinitionId = definitionId;
        }
    }
}