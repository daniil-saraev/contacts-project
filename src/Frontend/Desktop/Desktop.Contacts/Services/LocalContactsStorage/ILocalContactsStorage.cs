namespace Desktop.Contacts.Services
{
    internal interface ILocalContactsStorage
    {
        Task Save(UnitOfWorkState unitOfWorkState);

        Task<UnitOfWorkState?> Load();
    }
}
