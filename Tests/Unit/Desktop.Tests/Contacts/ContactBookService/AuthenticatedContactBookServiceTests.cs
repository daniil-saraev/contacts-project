using Core.Contacts.Models;
using Desktop.Common.Exceptions;
using Desktop.Contacts.Models;
using Desktop.Contacts.Services;
using NuGet.ContentModel;

namespace Desktop.Tests.Contacts;

public class AuthenticatedContactBookServiceTests
{
    private readonly AuthenticatedContactBookService _contactBookService;
    private readonly Mock<ISyncService> _syncService;
    private readonly Mock<ILocalContactsStorage> _storage;
    private readonly ContactsUnitOfWork _unitOfWork;

    public AuthenticatedContactBookServiceTests()
    {
        _syncService = new Mock<ISyncService>();
        _storage = new Mock<ILocalContactsStorage>();
        _storage.Setup(storage => storage.Save(It.IsAny<UnitOfWorkState>())).Returns(Task.CompletedTask);
        _storage.Setup(_storage => _storage.Load()).ReturnsAsync(It.IsAny<UnitOfWorkState>());
        _unitOfWork = new ContactsUnitOfWork();
        _contactBookService = new AuthenticatedContactBookService(_syncService.Object, _storage.Object, _unitOfWork);
    }

    #region GetAllContacts
    [Fact]
    public async Task GetAllContactsSuccessfulTest()
    {
        // Arrange
        var pulledContacts = new List<ContactData>()
        {
            new ContactData { Id = Guid.NewGuid().ToString() }
        };
        _syncService.Setup(sync => sync.Pull()).ReturnsAsync(pulledContacts);

        // Act
        var returnContacts = await _contactBookService.GetAllContacts();

        // Assert
        Assert.True(_unitOfWork.ExistingContacts.First().Id == pulledContacts.First().Id);
        Assert.True(pulledContacts.First().Id == returnContacts.First().Id);
        _storage.Verify(storage => storage.Save(_unitOfWork.UnitOfWorkState));
    }

    [Fact]
    public async Task GetAllContactsIfSyncServiceThrowsExceptionTest()
    {
        // Arrange
        UnitOfWorkState state = new UnitOfWorkState()
        {
            ExistingUnits = new List<ContactUnit>() { new ContactUnit(new ContactData { Id = Guid.NewGuid().ToString() }, State.New) }
        };
        _unitOfWork.UnitOfWorkState = state;
        _syncService.Setup(sync => sync.Pull()).ThrowsAsync(new SyncingWithRemoteRepositoryException());

        // Act
        var returnContacts = await _contactBookService.GetAllContacts();

        // Assert
        Assert.True(returnContacts.First().Id == state.ExistingUnits.First().Id);
        _storage.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllContactsIfStorageThrowsExceptionTest()
    {
        // Arrange
        var pulledContacts = new List<ContactData>()
        {
            new ContactData { Id = Guid.NewGuid().ToString() }
        };
        _syncService.Setup(sync => sync.Pull()).ReturnsAsync(pulledContacts);
        _storage.Setup(storage => storage.Save(It.IsAny<UnitOfWorkState>())).ThrowsAsync(new WritingDataException());

        // Act
        var returnContacts = await _contactBookService.GetAllContacts();

        // Assert
        Assert.True(pulledContacts.First().Id == returnContacts.First().Id);
        _storage.Verify(storage => storage.Save(_unitOfWork.UnitOfWorkState));
    }
    #endregion

    #region LoadContacts
    [Fact]
    public async Task LoadContactsIfSyncServiceThrowsException()
    {
        // Arrange
        UnitOfWorkState state = new UnitOfWorkState()
        {
            ExistingUnits = new List<ContactUnit>() { new ContactUnit(new ContactData { Id = Guid.NewGuid().ToString() }, State.New) }
        };
        _storage.Setup(storage => storage.Load()).ReturnsAsync(state);
        _syncService.Setup(sync => sync.Push(It.IsAny<UnitOfWorkState>())).ThrowsAsync(new SyncingWithRemoteRepositoryException());

        // Act & Assert
        await Assert.ThrowsAsync<SyncingWithRemoteRepositoryException>(async () => await _contactBookService.LoadContacts());
        Assert.Equal(_unitOfWork.UnitOfWorkState, state);
    }

    [Fact]
    public async Task LoadContactsIfStorageThrowsException()
    {
        // Arrange
        var pulledContacts = new List<ContactData>()
        {
            new ContactData { Id = Guid.NewGuid().ToString() }
        };
        _syncService.Setup(sync => sync.Pull()).ReturnsAsync(pulledContacts);
        _storage.Setup(storage => storage.Load()).ThrowsAsync(new ReadingDataException());

        // Act & Assert
        await Assert.ThrowsAsync<ReadingDataException>(async () => await _contactBookService.LoadContacts());
        Assert.True(_unitOfWork.ExistingContacts.First().Id == pulledContacts.First().Id);
    }
    #endregion

    #region SaveContacts
    [Fact]
    public async Task SaveContactsSuccessfulTest()
    {
        // Arrange
        UnitOfWorkState state = new UnitOfWorkState()
        {
            ExistingUnits = new List<ContactUnit>() { new ContactUnit(new ContactData { Id = Guid.NewGuid().ToString() }, State.New) }
        };
        _unitOfWork.UnitOfWorkState = state;
        _syncService.Setup(sync => sync.Push(It.IsAny<UnitOfWorkState>())).Returns(Task.CompletedTask);

        // Act
        await _contactBookService.SaveContacts();
     
        // Assert
        _syncService.Verify(sync => sync.Push(state));
        _storage.Verify(storage => storage.Save(state));
    }

    [Fact]
    public async Task SaveContactsIfSyncServiceThrowsExceptionTest()
    {
        // Arrange
        UnitOfWorkState state = new UnitOfWorkState()
        {
            ExistingUnits = new List<ContactUnit>() { new ContactUnit(new ContactData { Id = Guid.NewGuid().ToString() }, State.New) }
        };
        _unitOfWork.UnitOfWorkState = state;
        _syncService.Setup(sync => sync.Push(It.IsAny<UnitOfWorkState>())).ThrowsAsync(new SyncingWithRemoteRepositoryException());

        // Act & Assert
        await Assert.ThrowsAsync<SyncingWithRemoteRepositoryException>(async() => await _contactBookService.SaveContacts());
        _storage.Verify(storage => storage.Save(state));
    }
    #endregion
}