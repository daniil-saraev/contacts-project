using Desktop.Services.Data.UnitOfWork;

namespace DesktopTests.Data.UnitOfWork
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void RegisterSyncedEntityIfEntityWasNewTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact>{ contact }
            };

            // Act
            unitOfWork.RegisterSyncedEntity(contact);

            // Assert
            Assert.Contains<Contact>(contact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.NewEntities); 
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities); 
        }

        [Fact]
        public void RegisterSyncedEntityIfEntityWasRemovedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                RemovedEntities = new List<Contact>{ contact }
            };

            // Act
            unitOfWork.RegisterSyncedEntity(contact);

            // Assert
            Assert.DoesNotContain<Contact>(contact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.NewEntities); 
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities); 
        }


        [Fact]
        public void RegisterNewEntityIfEntityIsUniqueTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();

            // Act
            unitOfWork.RegisterNewEntity(contact);

            // Assert
            Assert.Contains<Contact>(contact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities); 
        }

        [Fact]
        public void RegisterNewEntityIfEntityWasAddedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterNewEntity(contact);

            // Assert
            Assert.Single(unitOfWork.NewEntities, contact);
        }

        [Fact]
        public void RegisterNewEntityIfEntityWasSyncedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterNewEntity(contact);

            // Assert
            Assert.Single(unitOfWork.SyncedEntities, contact);
        }

        [Fact]
        public void RegisterRemovedEntityIfEntityWasSyncedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterRemovedEntity(contact);

            // Assert
            Assert.Contains<Contact>(contact, unitOfWork.RemovedEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.SyncedEntities);
        }

        [Fact]
        public void RegisterRemovedEntityIfEntityWasAddedOrChangedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact contact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { contact }
            };

            // Act
            unitOfWork.RegisterRemovedEntity(contact);

            // Assert
            Assert.DoesNotContain<Contact>(contact, unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(contact, unitOfWork.RemovedEntities);
        }

        [Fact]
        public void RegisterUpdatedEntityIfEntityWasSyncedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact initialContact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact>{ initialContact }
            };
            Contact updatedContact = new Contact(initialContact.UserId, initialContact.Id)
            {
                Description = "updated contact"
            };

            // Act
            unitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);

            // Assert
            Assert.DoesNotContain<Contact>(initialContact, unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(updatedContact, unitOfWork.SyncedEntities); 
            Assert.Contains<Contact>(initialContact, unitOfWork.RemovedEntities);  
            Assert.Contains<Contact>(updatedContact, unitOfWork.NewEntities);          
        }

        [Fact]
        public void RegisterUpdatedEntityIfEntityWasAddedOrChangedBeforeTest()
        {
            // Arrange
            UnitOfWork<Contact> unitOfWork = new UnitOfWork<Contact>();
            Contact initialContact = new Contact();
            unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact>{ initialContact }
            };
            Contact updatedContact = new Contact(initialContact.UserId, initialContact.Id)
            {
                Description = "updated contact"
            };

            // Act
            unitOfWork.RegisterUpdatedEntity(initialContact, updatedContact);

            // Assert
            Assert.DoesNotContain<Contact>(initialContact, unitOfWork.NewEntities);
            Assert.Contains<Contact>(updatedContact, unitOfWork.NewEntities); 
            Assert.DoesNotContain<Contact>(initialContact, unitOfWork.RemovedEntities);        
        }
    }
}
