namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public interface IEffectProcessor
    {
        EffectType Type { get; }
        
        
        void Process(Characters.Character character, Effect effect);
    }
}