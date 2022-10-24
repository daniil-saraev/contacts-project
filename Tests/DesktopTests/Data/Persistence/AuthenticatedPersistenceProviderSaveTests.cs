using Desktop.Commands.Contacts.LoadCommand;
using Desktop.Exceptions;
using Desktop.Services.Data.Persistence;
using Desktop.Services.Data.Persistence.DiskProvider;
using Desktop.Services.Data.Persistence.RemoteRepositoryProvider;
using Desktop.Services.Data.UnitOfWork;
using Moq;

namespace DesktopTests.Data.Persistence
{
    public class AuthenticatedPersistenceProviderSaveTests
    {
        private readonly Contact _contact;
        private readonly UnitOfWork<Contact> _unitOfWork;
        private readonly Mock<IDiskProvider> _diskProviderMock;
        private readonly Mock<IRemoteRepositoryProvider> _remoteRepositoryProviderMock;
        private readonly AuthenticatedPersistenceProvider _authenticatedPersistenceProvider;

        public AuthenticatedPersistenceProviderSaveTests()
        {
            _contact = new Contact();
            _unitOfWork = new UnitOfWork<Contact>();
            _diskProviderMock = new Mock<IDiskProvider>();
            _remoteRepositoryProviderMock = new Mock<IRemoteRepositoryProvider>();
            _authenticatedPersistenceProvider = new AuthenticatedPersistenceProvider(_unitOfWork, _diskProviderMock.Object, _remoteRepositoryProviderMock.Object);
        }


        [Fact]
        public async Task SaveContactsAsyncTest()
        {
            // Arrange
            _unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>
            {
                NewEntities = new List<Contact> { _contact }
            };
            _diskProviderMock.Setup(disk => disk.TrySaveToDiskAsync(It.IsAny<UnitOfWorkState<Contact>>())).Returns(Task.CompletedTask);
            _remoteRepositoryProviderMock.Setup(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>())).Returns((Task.CompletedTask));

            // Act
            await _authenticatedPersistenceProvider.SaveContactsAsync();

            // Assert
            _remoteRepositoryProviderMock.Verify(repoitory => repoitory.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>()));           
            Assert.Contains<Contact>(_contact, _unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(_contact, _unitOfWork.NewEntities);
            _diskProviderMock.Verify(disk => disk.TrySaveToDiskAsync(It.IsAny<UnitOfWorkState<Contact>>()));
        }

        [Fact]
        public async Task SaveContactsAsyncIfRepositoryProviderThrowsExceptionTest()
        {
            // Arrange
            _unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { _contact }
            };
            _diskProviderMock.Setup(disk => disk.TrySaveToDiskAsync(It.IsAny<UnitOfWorkState<Contact>>())).Returns(Task.CompletedTask);
            _remoteRepositoryProviderMock.Setup(provider => provider.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>())).ThrowsAsync(new SyncingWithRemoteRepositoryException());

            // Act & Assert
            await Assert.ThrowsAsync<SyncingWithRemoteRepositoryException>(async () => await _authenticatedPersistenceProvider.SaveContactsAsync());
            Assert.Contains<Contact>(_contact, _unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(_contact, _unitOfWork.SyncedEntities);
            _diskProviderMock.Verify(disk => disk.TrySaveToDiskAsync(It.IsAny<UnitOfWorkState<Contact>>()));
            
        }

        [Fact]
        public async Task SaveContactsAsyncIfDiskProviderThrowsExceptionTest()
        {
            // Arrange
            _unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { _contact }
            };
            _diskProviderMock.Setup(disk => disk.TrySaveToDiskAsync(It.IsAny<UnitOfWorkState<Contact>>())).ThrowsAsync(new WritingDataException());
            _remoteRepositoryProviderMock.Setup(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>())).Returns((Task.CompletedTask));

            // Act & Assert
            await Assert.ThrowsAsync<WritingDataException>(async () => await _authenticatedPersistenceProvider.SaveContactsAsync());
            _remoteRepositoryProviderMock.Verify(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>()));
            Assert.Contains<Contact>(_contact, _unitOfWork.SyncedEntities);
            Assert.DoesNotContain<Contact>(_contact, _unitOfWork.NewEntities);          
        }

        [Fact]
        public async Task SaveContactsAsyncIfDiskAndRepositoryProvidersThrowExceptionsTest()
        {
            // Arrange
            _unitOfWork.UnitOfWorkState = new UnitOfWorkState<Contact>()
            {
                NewEntities = new List<Contact> { _contact }
            };
            _diskProviderMock.Setup(disk => disk.TrySaveToDiskAsync(It.IsAny<UnitOfWorkState<Contact>>())).ThrowsAsync(new WritingDataException());
            _remoteRepositoryProviderMock.Setup(provider => provider.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>())).ThrowsAsync(new SyncingWithRemoteRepositoryException());

            // Act & Assert
            await Assert.ThrowsAsync<WritingDataException>(async () => await _authenticatedPersistenceProvider.SaveContactsAsync());
            _remoteRepositoryProviderMock.Verify(repository => repository.TryPushChangesToRemoteRepositoryAsync(It.IsAny<UnitOfWorkState<Contact>>()));
            Assert.Contains<Contact>(_contact, _unitOfWork.NewEntities);
            Assert.DoesNotContain<Contact>(_contact, _unitOfWork.SyncedEntities);          
        }
    }
}
