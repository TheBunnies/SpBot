using System.Threading.Tasks;

namespace SpBot.Core
{
    public interface IServerInfoNotifier
    {
        Task ChangeStatus();
    }
}
