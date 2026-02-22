using System.Threading.Tasks;

namespace _Project.Scripts.Infrastructure.FSM.ProjectSM.States
{
    public interface IProjectState
    {
        Task Enter();
        Task Exit();
    }
}