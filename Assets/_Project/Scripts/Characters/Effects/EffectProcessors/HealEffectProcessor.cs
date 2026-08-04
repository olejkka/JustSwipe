namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class HealEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.Heal;

        
        public void Process(Character character, Effect effect)
        {
            character.ChangeHealth(effect.Parameter);
        }
    }
}