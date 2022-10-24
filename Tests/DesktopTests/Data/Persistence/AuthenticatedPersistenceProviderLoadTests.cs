using Desktop.Exceptions;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.Persistence.RemoteRepositoryProvider;
using Desktop.Services.Data.UnitOfWork;
using Moq;

namespace DesktopTests.Data.Persistence
{
    public class AuthenticatedPersistenceProviderLoadTests
    {
        private readonly Contact _contact;
        private readonly UnitOfWork<Contact> _unitOfWork;
        private readonly Mock<IDiskProvider> _diskProviderMock;
        private readonly Mock<IRemoteRepositoryProvider> _remoteRepositoryProviderMock;
        private readonly AuthenticatedPersistenceProvider _authenticatedPersistenceProvider;
        private readonly UnitOfWorkState<Contact> _unitOfWorkStateFormDisk;
        private readonly UnitOfWorkState<Contact> _unitOfWorkStateFromRemoteRepository;

        public AuthenticatedPersistenceProviderLoadTests()
        {
            _contact = new Contact();
            _unitOfWork = new UnitOfWork<Contact>();
            _diskProviderMock = new Mock<IDiskProvider>();
            _remoteRepositoryProviderMock = new Mock<IRemoteRepositoryProvider>();
            _authenticatedPersistenceProvider = new AuthenticatedPersistenceProvider(_unitOfWork, _diskProviderMock.Object, _remoteRepositoryProviderMock.Object);
            _unitOfWorkStateFormDisk = new UnitOfWorkState<Contact>() { NewEntities = new List<Contact> { _contact } };
            _unitOfWorkStateFromRemoteRepository = new UnitOfWorkState<Contact>() { SyncedEntities = new List<Contact> { _contact } };
        }

        [Fact]
        public async Task LoadContactsAsyncTest()
        {
            // Arrange
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.FromResult(_unitOfWorkStateFormDisk));
            _remoteRepositoryProviderMock.Setup(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>())).Returns(Task.CompletedTask);
            _remoteRepositoryProviderMock.Setup(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync()).Returns(Task.FromResult(_unitOfWorkStateFromRemoteRepository));

            // Act
            var contacts = await _authenticatedPersistenceProvider.LoadContactsAsync();

            // Assert
            Assert.Contains(_contact, _unitOfWork.SyncedEntities);
            Assert.Contains(_contact, contacts);           
            _diskProviderMock.Verify(disk => disk.TryLoadFromDiskAsync());
            _remoteRepositoryProviderMock.Verify(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>()));
            _remoteRepositoryProviderMock.Verify(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync());         
        }

        [Fact]
        public async Task LoadContactsAsyncIfDiskProviderReturnsNullTest()
        {
            // Arrange
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.Factory.StartNew<UnitOfWorkState<Contact>?>(() => { return null; }));
            _remoteRepositoryProviderMock.Setup(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync()).Returns(Task.FromResult(_unitOfWorkStateFromRemoteRepository));

            // Act
            var contacts = await _authenticatedPersistenceProvider.LoadContactsAsync();

            // Assert
            Assert.Contains(_contact, _unitOfWork.SyncedEntities);
            Assert.Contains(_contact, contacts);
            _diskProviderMock.Verify(disk => disk.TryLoadFromDiskAsync());
            _remoteRepositoryProviderMock.Verify(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync());
            _remoteRepositoryProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task LoadContactsAsyncIfDiskAndRepositoryProvidersReturnNullTest()
        {
            // Arrange
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.Factory.StartNew<UnitOfWorkState<Contact>?>(() => { return null; }));
            _remoteRepositoryProviderMock.Setup(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync()).Returns(Task.Factory.StartNew<UnitOfWorkState<Contact>?>(() => { return null; }));

            // Act
            var contacts = await _authenticatedPersistenceProvider.LoadContactsAsync();

            // Assert
            Assert.NotNull(_unitOfWork.UnitOfWorkState);
            Assert.NotNull(contacts);
            _diskProviderMock.Verify(disk => disk.TryLoadFromDiskAsync());
            _remoteRepositoryProviderMock.Verify(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync());
            _remoteRepositoryProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task LoadContactsAsyncIfDiskProviderThrowsExceptionTest()
        {
            // Arrange
            _diskProviderMock.Setup(disk =>  disk.TryLoadFromDiskAsync()).ThrowsAsync(new ReadingDataException());
            _remoteRepositoryProviderMock.Setup(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync()).Returns(Task.FromResult(_unitOfWorkStateFromRemoteRepository));

            // Act & Assert
            await Assert.ThrowsAsync<ReadingDataException>(async () => await _authenticatedPersistenceProvider.LoadContactsAsync());
            Assert.NotNull(_unitOfWork.UnitOfWorkState);
        }

        [Fact]
        public async Task LoadContactsAsyncIfRepositoryProviderThrowsExceptionTest()
        {
            // Arrange
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).Returns(Task.FromResult(_unitOfWorkStateFormDisk));
            _remoteRepositoryProviderMock.Setup(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>())).ThrowsAsync(new SyncingWithRemoteRepositoryException());

            // Act & Assert
            await Assert.ThrowsAsync<SyncingWithRemoteRepositoryException>(async () => await _authenticatedPersistenceProvider.LoadContactsAsync());
            Assert.NotNull(_unitOfWork.UnitOfWorkState);
        }

        [Fact]
        public async Task LoadContactsAsyncIfDiskAndRepositoryProvidersThrowExceptionsTest()
        {
            // Arrange
            _diskProviderMock.Setup(disk => disk.TryLoadFromDiskAsync()).ThrowsAsync(new ReadingDataException());
            _remoteRepositoryProviderMock.Setup(repositoty => repositoty.TryLoadFromRemoteRepositoryAsync()).ThrowsAsync(new SyncingWithRemoteRepositoryException());

            // Act & Assert
            await Assert.ThrowsAsync<ReadingDataException>(async () => await _authenticatedPersistenceProvider.LoadContactsAsync());
            Assert.NotNull(_unitOfWork.UnitOfWorkState);
        }
    }
}
