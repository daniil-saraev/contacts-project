namespace Desktop.Contacts.Services
{
    public interface IPersistenceProvider
    {
        Task LoadContacts();

        Task SaveContacts();
    }
}
