using Desktop.Commands.Contacts;
using Desktop.Commands.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Desktop.ViewModels.Contacts
{
    public class HomeViewModel : BaseViewModel
    {
        private IEnumerable<ContactViewModel> _contacts;

        public ObservableCollection<ContactViewModel> Contacts => 
            (ObservableCollection<ContactViewModel>)(_contacts ?? (_contacts = new Collection<ContactViewModel>()));

        public ContactViewModel? SelectedContact { get; set; }
        
        private ICommand LoadContacts { get; }
        public ICommand DeleteContact { get; }
        public ICommand NavigateToInfoView { get; }
        public ICommand NavigateToEditView { get; }
        public ICommand NavigateToAddVIew { get; }

        public HomeViewModel()
        {
            LoadContacts = new LoadContactsCommand(ref _contacts);          
            DeleteContact = new DeleteContactCommand(SelectedContact);
            NavigateToInfoView = new NavigateCommand(new ContactInfoViewModel(SelectedContact), (o) => SelectedContact != null);
            NavigateToEditView = new NavigateCommand(new ContactEditViewModel(SelectedContact), (o) => SelectedContact != null);
            NavigateToAddVIew = new NavigateCommand(new ContactAddViewModel());
            LoadContacts.Execute(null);
        }
    }
}
