using System.Threading.Tasks;

namespace SpBot.Core
{
    //Этот код принадлежит Алексею Уланову a.k.a. Ultra_Rabbit'у
    public interface IServerInfoNotifier
    {
        Task ChangeStatus();
    }
}
