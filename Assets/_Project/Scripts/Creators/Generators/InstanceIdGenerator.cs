using _Project.Scripts.Characters;

namespace _Project.Scripts.Creators.Generators
{
    public class InstanceIdGenerator
    {
        private int _nextId = 1;
        
        
        public int Next() => _nextId++;
    }
}