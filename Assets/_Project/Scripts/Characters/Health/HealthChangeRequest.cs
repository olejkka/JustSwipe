namespace _Project.Scripts.Characters.Health
{
    public readonly struct HealthChangeRequest
    {
        public HealthChangeSource Source { get; }
        public Character Target { get; }
        public HealthChangeType Type { get; }
        public int Amount { get; }
        public float DamageMultiplier { get; }

        
        public static HealthChangeRequest Damage(
            HealthChangeSource source,
            Character target,
            int amount,
            float damageMultiplier = 1f) =>
            new(source, target, HealthChangeType.Damage, amount, damageMultiplier);

        public static HealthChangeRequest Heal(
            HealthChangeSource source,
            Character target,
            int amount,
            float damageMultiplier = 1f) =>
            new(source, target, HealthChangeType.Heal, amount, damageMultiplier);

        private HealthChangeRequest(
            HealthChangeSource source,
            Character target,
            HealthChangeType type,
            int amount,
            float damageMultiplier)
        {
            Source = source;
            Target = target;
            Type = type;
            Amount = amount;
            DamageMultiplier = damageMultiplier;
        }
    }
}
