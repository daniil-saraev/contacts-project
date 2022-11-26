namespace Desktop.Contacts.Persistence
{
    public interface IPersistenceProvider
    {
        Task LoadContacts();

        Task SaveContacts();
    }
}
