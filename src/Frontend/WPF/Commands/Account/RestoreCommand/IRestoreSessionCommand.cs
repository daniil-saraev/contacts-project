using System.Threading.Tasks;

namespace Desktop.Commands.Account.RestoreCommand
{
    public interface IRestoreSessionCommand
    {
        Task Execute();
    }
}