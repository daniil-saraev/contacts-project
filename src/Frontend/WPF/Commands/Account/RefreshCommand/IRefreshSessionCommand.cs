using System.Threading.Tasks;

namespace Desktop.Commands.Account.Refresh
{
    public interface IRefreshSessionCommand
    {
        Task Refresh();
    }
}