using Desktop.Exceptions;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.UnitOfWork;

namespace DesktopTests.Data.Persistence
{
    public class NotAuthenticatedPersistenceProviderLoadTests
    {
        private readonly Mock<IDiskProvider> _diskProviderMock;

        public NotAuthenticatedPersistenceProviderLoadTests()
        {
            _diskProviderMock = new Mock<IDiskProvider>();
        }

        [Fact]
        public async Task LoadContactsAsyncTest()
        {
            // Arrange
            Contact contact = new Contact();
            UnitOfWorkState<Contact>? unitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { contact }
            };
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.Factory.StartNew<UnitOfWorkState<Contact>?>(() => { return unitOfWorkState; }));

            NotAuthenticatedPersistenceProvider persistenceProvider = new NotAuthenticatedPersistenceProvider(new UnitOfWork<Contact>(), _diskProviderMock.Object);

            // Act
            var contacts = await persistenceProvider.LoadContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.Contains<Contact>(contact, contacts);
        }

        [Fact]
        public async Task LoadContactsAsyncIfNullTest()
        {
            // Arrange
            UnitOfWorkState<Contact>? unitOfWorkState = null;
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.Factory.StartNew<UnitOfWorkState<Contact>?>(() => { return unitOfWorkState; }));

            NotAuthenticatedPersistenceProvider persistenceProvider = new NotAuthenticatedPersistenceProvider(new UnitOfWork<Contact>(), _diskProviderMock.Object);

            // Act
            var contacts = await persistenceProvider.LoadContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.Empty(contacts);            
        }

        [Fact]
        public async Task LoadContactsAsyncIfThrowsException()
        {
            // Arrange
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).ThrowsAsync(new ReadingDataException());

            NotAuthenticatedPersistenceProvider persistenceProvider = new NotAuthenticatedPersistenceProvider(new UnitOfWork<Contact>(), _diskProviderMock.Object);

            // Act
            var contacts = await persistenceProvider.LoadContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.Empty(contacts);
        }
    }
}
