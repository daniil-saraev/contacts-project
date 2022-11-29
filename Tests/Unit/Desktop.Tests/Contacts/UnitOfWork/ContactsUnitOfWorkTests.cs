using Core.Contacts.Models;
using Core.Contacts.Requests;
using Desktop.Contacts.Models;
using Desktop.Contacts.Services;

namespace Desktop.Tests.Contacts
{
    public class ContactsUnitOfWorkTests
    {
        private readonly ContactsUnitOfWork _unitOfWork;

        public ContactsUnitOfWorkTests()
        {
            _unitOfWork = new ContactsUnitOfWork();
        }

        #region AddContacts
        [Fact]
        public void AddContacts()
        {      
            // Arrange
            ContactData syncedContact = new ContactData() { Id = Guid.NewGuid().ToString() };
            ContactData newContact = new ContactData() { Id = Guid.NewGuid().ToString() };
            ContactData updatedContact = new ContactData { Id = Guid.NewGuid().ToString() };
            ContactData deletedContact = new ContactData { Id = Guid.NewGuid().ToString() };
            List<ContactData> contacts = new List<ContactData>() { syncedContact, newContact, updatedContact, deletedContact };
            _unitOfWork.UnitOfWorkState = new UnitOfWorkState()
            {
                ExistingUnits = new List<ContactUnit>()
                {
                    new ContactUnit(syncedContact, State.Synced),
                    new ContactUnit(updatedContact, State.Changed)
                },
                PendingDeleteRequests = new List<DeleteContactRequest>() { new DeleteContactRequest() { Id = deletedContact.Id} }
            };

            // Act
            _unitOfWork.AddContacts(contacts);

            // Assert
            Assert.Single<ContactUnit>(_unitOfWork.UnitOfWorkState.ExistingUnits, 
                                        unit => unit.Id == syncedContact.Id && unit.State == State.Synced);                                                                                   
            Assert.Single<ContactUnit>(_unitOfWork.UnitOfWorkState.ExistingUnits,
                                        unit => unit.Id == newContact.Id && unit.State == State.Synced);                                                                                         
            Assert.Single<ContactUnit>(_unitOfWork.UnitOfWorkState.ExistingUnits,
                                        unit => unit.Id == updatedContact.Id && unit.State == State.Changed);                                                                                           
            Assert.Single<DeleteContactRequest>(_unitOfWork.UnitOfWorkState.PendingDeleteRequests,
                                        request => request.Id == deletedContact.Id);
            Assert.Null(_unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit => unit.Id == deletedContact.Id));                                                                                       
        }
        #endregion

        #region AddContact
        [Fact]
        public void AddContactTest()
        {
            // Arrange
            AddContactRequest request = new AddContactRequest
            {
                FirstName = "User",
                PhoneNumber = "+79998887766"
            };

            // Act
            _unitOfWork.AddContact(request);

            // Assert
            var unit = _unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit => 
                                                    unit.Contact.PhoneNumber == request.PhoneNumber);
            Assert.NotNull(unit);
            Assert.True(unit.State == State.New);
            Assert.False(_unitOfWork.UnitOfWorkState.IsSynced);                                       
        }
        #endregion

        #region UpdateContact
        [Fact]
        public void UpdateContactIfContactIsSyncedTest()
        {
            // Arrange
            ContactData data = new ContactData() { Id = Guid.NewGuid().ToString() }; 
            UnitOfWorkState state = new UnitOfWorkState()
            {
                ExistingUnits = new List<ContactUnit>()
                { 
                    new ContactUnit(data, State.Synced)
                }
            };
            _unitOfWork.UnitOfWorkState = state;
            UpdateContactRequest request = new UpdateContactRequest
            {
                Id = data.Id,
                PhoneNumber = "+79998887766"
            };

            // Act
            _unitOfWork.UpdateContact(request);

            // Assert
            var unit = _unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit =>
                                                    unit.Id == data.Id);
            Assert.NotNull(unit);
            Assert.True(unit.Contact.PhoneNumber == request.PhoneNumber);
            Assert.True(unit.State == State.Changed);
            Assert.False(_unitOfWork.UnitOfWorkState.IsSynced);   
        }  

        public void UpdateContactIfContactIsNewTest()
        {
            // Arrange
            ContactData data = new ContactData() { Id = Guid.NewGuid().ToString() }; 
            UnitOfWorkState state = new UnitOfWorkState()
            {
                ExistingUnits = new List<ContactUnit>()
                { 
                    new ContactUnit(data, State.New)
                }
            };
            _unitOfWork.UnitOfWorkState = state;
            UpdateContactRequest request = new UpdateContactRequest
            {
                Id = data.Id,
                PhoneNumber = "+79998887766"
            };

            // Act
            _unitOfWork.UpdateContact(request);

            // Assert
            var unit = _unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit =>
                                                    unit.Id == data.Id);
            Assert.NotNull(unit);
            Assert.True(unit.Contact.PhoneNumber == data.PhoneNumber);
            Assert.True(unit.State == State.New);
            Assert.False(_unitOfWork.UnitOfWorkState.IsSynced);   
        } 
        #endregion

        #region DeleteContact
        [Fact]
        public void DeleteContactIfContactIsSyncedTest()
        {
            // Arrange
            ContactData data = new ContactData() { Id = Guid.NewGuid().ToString() }; 
            UnitOfWorkState state = new UnitOfWorkState()
            {
                ExistingUnits = new List<ContactUnit>()
                { 
                    new ContactUnit(data, State.Synced)
                }
            };
            _unitOfWork.UnitOfWorkState = state;
            DeleteContactRequest request = new DeleteContactRequest()
            {
                Id = data.Id
            };

            // Act
            _unitOfWork.DeleteContact(request);

            // Assert
            Assert.Null(_unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit => unit.Id == data.Id));
            Assert.Contains(request, _unitOfWork.UnitOfWorkState.PendingDeleteRequests);
            Assert.False(_unitOfWork.UnitOfWorkState.IsSynced);   
        }

        [Fact]
        public void DeleteContactIfContactIsChangedTest()
        {
            // Arrange
            ContactData data = new ContactData() { Id = Guid.NewGuid().ToString() }; 
            UnitOfWorkState state = new UnitOfWorkState()
            {
                ExistingUnits = new List<ContactUnit>()
                { 
                    new ContactUnit(data, State.Changed)
                }
            };
            _unitOfWork.UnitOfWorkState = state;
            DeleteContactRequest request = new DeleteContactRequest()
            {
                Id = data.Id
            };

            // Act
            _unitOfWork.DeleteContact(request);

            // Assert
            Assert.Null(_unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit => unit.Id == data.Id));
            Assert.Contains(request, _unitOfWork.UnitOfWorkState.PendingDeleteRequests);
            Assert.False(_unitOfWork.UnitOfWorkState.IsSynced);   
        }

        [Fact]
        public void DeleteContactIfContactIsNewTest()
        {
            // Arrange
            ContactData data = new ContactData() { Id = Guid.NewGuid().ToString() }; 
            UnitOfWorkState state = new UnitOfWorkState()
            {
                ExistingUnits = new List<ContactUnit>()
                { 
                    new ContactUnit(data, State.New)
                }
            };
            _unitOfWork.UnitOfWorkState = state;
            DeleteContactRequest request = new DeleteContactRequest()
            {
                Id = data.Id
            };

            // Act
            _unitOfWork.DeleteContact(request);

            // Assert
            Assert.Null(_unitOfWork.UnitOfWorkState.ExistingUnits.Find(unit => unit.Id == data.Id));
            Assert.DoesNotContain(request, _unitOfWork.UnitOfWorkState.PendingDeleteRequests);
            Assert.True(_unitOfWork.UnitOfWorkState.IsSynced); 
        }
        #endregion
    }
}
