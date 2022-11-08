using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.App.Commands.Contacts.LoadCommand
{
    public interface ILoadContactsCommand
    {
        Task Execute();
    }
}
