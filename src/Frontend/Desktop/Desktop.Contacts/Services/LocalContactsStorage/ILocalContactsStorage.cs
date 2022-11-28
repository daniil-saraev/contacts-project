using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Desktop.Contacts.Services
{
    internal interface ILocalContactsStorage
    {
        public Task Save(UnitOfWorkState unitOfWorkState);

        public Task<UnitOfWorkState?> Load();
    }
}
