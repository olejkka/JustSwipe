using _Project.Scripts.Characters;

namespace _Project.Scripts.Creators.Generators
{
    public class CharacterInstanceIdGenerator
    {
        private int _nextId = 1;
        
        public int Next()
        {
            return _nextId++;
        }
    }
}