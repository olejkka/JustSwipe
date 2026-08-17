using _Project.Scripts.Characters;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class ApplyEffectEvent
    {
        public Team TargetTeam { get; }
        public Team OwnerTeam { get; }
        public string DefinitionId { get; }

        
        public ApplyEffectEvent(Team targetTeam, Team ownerTeam, string definitionId)
        {
            TargetTeam = targetTeam;
            OwnerTeam = ownerTeam;
            DefinitionId = definitionId;
        }
    }
}
