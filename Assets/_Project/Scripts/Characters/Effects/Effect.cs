using System;
using _Project.Scripts.Characters;

namespace _Project.Scripts.Characters.Effects
{
    [Serializable]
    public struct Effect
    {
        public string DefinitionId;
        public int InstanceId;
        public EffectType Type;
        public int Parameter;
        public int RemainingTurns;
        public Team OwnerTeam;
        
        
        public Effect(
            EffectType type,
            int parameter,
            int remainingTurns,
            string definitionId,
            int instanceId,
            Team ownerTeam)
        {
            DefinitionId = definitionId;
            InstanceId = instanceId;
            Type = type;
            Parameter = parameter;
            RemainingTurns = remainingTurns;
            OwnerTeam = ownerTeam;
        }
    }
}
