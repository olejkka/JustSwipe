using System;

namespace _Project.Scripts.Characters.Structs___Enums
{
    [Serializable]
    public struct CharacterEffect
    {
        public string DefinitionId;
        public int InstanceId;
        public EffectType Type;
        public int Parameter;
        public int RemainingTurns;
        
        
        public CharacterEffect(
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