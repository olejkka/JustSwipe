namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class DealDamageEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.DealDamage;

        
        public void Process(Character character, Effect effect)
        {
            character.ChangeHealth(-effect.Parameter);
        }
    }
}