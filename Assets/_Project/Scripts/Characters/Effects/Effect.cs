using System;

namespace _Project.Scripts.Characters.StructsEnums
{
    [Serializable]
    public struct Effect
    {
        public string DefinitionId;
        public int InstanceId;
        public EffectType Type;
        public int Parameter;
        public int RemainingTurns;
        
        
        public Effect(
            EffectType type,
            int parameter,
            int remainingTurns,
            string definitionId,
            int instanceId)
        {
            DefinitionId = definitionId;
            InstanceId = instanceId;
            Type = type;
            Parameter = parameter;
            RemainingTurns = remainingTurns;
        }
    }
}