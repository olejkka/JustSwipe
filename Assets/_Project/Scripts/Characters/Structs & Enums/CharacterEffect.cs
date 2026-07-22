using System;

namespace _Project.Scripts.Characters.Structs___Enums
{
    [Serializable]
    public struct CharacterEffect
    {
        public EffectType Type;
        public int Parameter;
        public int RemainingTurns;
        
        
        public CharacterEffect(EffectType type, int parameter, int remainingTurns)
        {
            Type = type;
            Parameter = parameter;
            RemainingTurns = remainingTurns;
        }
    }
}