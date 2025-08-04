using _Project.Scripts.Characters;
using _Project.Scripts.Tiles;

namespace _Project.Scripts.Infrastructure
{
    public class GameState
    {
        public PositionsStorage PositionsStorage = new ();
        public CharactersStorage CharactersStorage = new();
    }
}