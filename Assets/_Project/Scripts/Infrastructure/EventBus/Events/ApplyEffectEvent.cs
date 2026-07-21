using _Project.Scripts.Characters.Structs;

namespace _Project.Scripts.Infrastructure.EventBus.Events
{
    public class ApplyEffectEvent
    {
        public Team Team { get; }
        public EffectType EffectType { get; }
        public int Parameter { get; }
        public int Turns { get; }

        
        public ApplyEffectEvent(Team team, EffectType effectType, int parameter, int turns)
        {
            Team = team;
            EffectType = effectType;
            Parameter = parameter;
            Turns = turns;
        }
    }
}