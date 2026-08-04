namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class DamageDecreaseEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.DamageDecrease;

        
        public void Process(Character character, Effect effect)
        {
            character.ChangeDamage(-effect.Parameter, affectBaseDamage: true);
        }
    }
}