using System.Threading.Tasks;

namespace Desktop.App.Commands.Account.RestoreCommand
{
    public interface IRestoreSessionCommand
    {
        Task Execute();
    }
}