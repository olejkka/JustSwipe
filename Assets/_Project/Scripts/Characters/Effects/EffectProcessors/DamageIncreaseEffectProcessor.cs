namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class DamageIncreaseEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.DamageIncrease;
        
        public void Process(Characters.Character character, Effect effect)
        {
            character.AddBonusDamage(effect.Parameter);
        }
    }
}