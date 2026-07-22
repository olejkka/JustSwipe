using _Project.Scripts.Characters.Structs;

namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class HealthIncreaseEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.HealthIncrease;
        
        public void Process(Character character, CharacterEffect effect)
        {
            character.AddBonusHealth(effect.Parameter);
        }
    }
}