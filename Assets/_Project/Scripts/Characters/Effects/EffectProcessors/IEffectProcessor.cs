using _Project.Scripts.Characters.Structs;

namespace _Project.Scripts.Characters.Effects.EffectProcessors
{
    public interface IEffectProcessor
    {
        EffectType Type { get; }
        
        
        void Process(Character character, CharacterEffect effect);
    }
}