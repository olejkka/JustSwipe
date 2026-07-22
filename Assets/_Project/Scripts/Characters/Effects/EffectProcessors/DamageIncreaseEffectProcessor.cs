using _Project.Scripts.Characters.Structs___Enums;

namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class DamageIncreaseEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.DamageIncrease;
        
        public void Process(Character character, CharacterEffect effect)
        {
            character.AddBonusDamage(effect.Parameter);
        }
    }
}