namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class DamageIncreaseEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.DamageIncrease;
        
        
        public void Process(Character character, Effect effect)
        {
            character.AddBonusDamage(effect.Parameter);
        }
    }
}