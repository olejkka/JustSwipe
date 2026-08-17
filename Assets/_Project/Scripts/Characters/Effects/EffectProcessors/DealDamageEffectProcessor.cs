using _Project.Scripts.Characters.Health;

namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class DealDamageEffectProcessor : IEffectProcessor
    {
        private readonly HealthChangeService _healthChangeService;

        public EffectType Type => EffectType.DealDamage;

        
        public DealDamageEffectProcessor(HealthChangeService healthChangeService)
        {
            _healthChangeService = healthChangeService;
        }

        public void Process(Character character, Effect effect)
        {
            _healthChangeService.Enqueue(
                HealthChangeRequest.Damage(
                    HealthChangeSource.FromEffect(effect),
                    character,
                    effect.Parameter));
        }
    }
}