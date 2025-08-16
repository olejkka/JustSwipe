namespace _Project.Scripts.FSM
{
    public class TurnService : ITurnService
    {
        public bool PlayerMoveFinished { get; set; }
        public bool BotMoveFinished { get; set; }
    }
}