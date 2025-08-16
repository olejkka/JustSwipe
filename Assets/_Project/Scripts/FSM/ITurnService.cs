namespace _Project.Scripts.FSM
{
    public interface ITurnService
    {
        bool PlayerMoveFinished { get; set; }
        bool BotMoveFinished { get; set; }
    }
}