using Desktop.Services.Data.UnitOfWork;
using DesktopApp.Exceptions;
using DesktopApp.Services.Data.Persistence;
using DesktopApp.Services.Data.Persistence.DiskProvider;
using DesktopApp.Services.Data.UnitOfWork;

namespace Desktop.Tests.Data.Persistence
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
            var contact = new Contact();
            UnitOfWorkState<Contact>? unitOfWorkState = new UnitOfWorkState<Contact>()
            {
                SyncedEntities = new List<Contact> { contact }
            };
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.Factory.StartNew(() => { return unitOfWorkState; }));

            var persistenceProvider = new NotAuthenticatedPersistenceProvider(new UnitOfWork<Contact>(), _diskProviderMock.Object);

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
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.Factory.StartNew(() => { return unitOfWorkState; }));

            var persistenceProvider = new NotAuthenticatedPersistenceProvider(new UnitOfWork<Contact>(), _diskProviderMock.Object);

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

            var persistenceProvider = new NotAuthenticatedPersistenceProvider(new UnitOfWork<Contact>(), _diskProviderMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ReadingDataException>(async () => await persistenceProvider.LoadContactsAsync());
        }
    }
}
