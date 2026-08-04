namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class HealthIncreaseEffectProcessor : IEffectProcessor
    {
        public EffectType Type => EffectType.HealthIncrease;
        
        public void Process(Characters.Character character, Effect effect)
        {
            character.AddBonusHealth(effect.Parameter);
        }
    }
}