using _Project.Scripts.Characters.Health;

namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public class HealEffectProcessor : IEffectProcessor
    {
        private readonly HealthChangeService _healthChangeService;

        public EffectType Type => EffectType.Heal;

        
        public HealEffectProcessor(HealthChangeService healthChangeService)
        {
            _healthChangeService = healthChangeService;
        }

        public void Process(Character character, Effect effect)
        {
            _healthChangeService.Enqueue(
                HealthChangeRequest.Heal(
                    HealthChangeSource.FromEffect(effect),
                    character,
                    effect.Parameter));
        }
    }
}