using System.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM.States
{
    public class InitializationState : IProjectState
    {
        public Task Enter()
        {
            // здесь будет инициализация SDK, загрузка сохранений и т.д.
            return Task.CompletedTask;
        }

        public Task Exit() => Task.CompletedTask;
    }
}