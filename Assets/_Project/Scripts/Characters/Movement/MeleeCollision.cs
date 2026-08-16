namespace _Project.Scripts.Characters.Movement
{
    public readonly struct MeleeCollision
    {
        public Character Attacker { get; }
        public Character Defender { get; }


        public MeleeCollision(Character attacker, Character defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
    }
}