using DatabaseApi;
using Desktop.ViewModels.Contacts;

namespace Desktop.Factories
{
    public static class ContactMVMFactory
    {
        public static Contact GetContactModel(ContactViewModel viewModel)
        {
            Contact contact = new Contact
            {
                Id = viewModel.Id,
                UserId = viewModel.UserId,
                FirstName = viewModel.FirstName,
                MiddleName = viewModel.MiddleName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Address = viewModel.Address,
                Description = viewModel.Description
            };
            return contact;
        }

        public static ContactViewModel GetContactViewModel(Contact model)
        {
            ContactViewModel contactViewModel = new ContactViewModel(model);
            return contactViewModel;
        }
    }
}
