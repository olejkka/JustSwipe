using _Project.Scripts.Characters.Effects;

namespace _Project.Scripts.Characters.Health
{
    public readonly struct HealthChangeSource
    {
        public HealthChangeSourceType Type { get; }
        public Character Character { get; }
        public Effect Effect { get; }

        public static HealthChangeSource None => default;

        public static HealthChangeSource FromCharacter(Character character) =>
            new(HealthChangeSourceType.Character, character, default);

        public static HealthChangeSource FromEffect(Effect effect) =>
            new(HealthChangeSourceType.Effect, null, effect);

        private HealthChangeSource(
            HealthChangeSourceType type,
            Character character,
            Effect effect)
        {
            Type = type;
            Character = character;
            Effect = effect;
        }
    }
}
