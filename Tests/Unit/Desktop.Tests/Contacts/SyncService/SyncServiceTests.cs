using Core.Contacts.Interfaces;
using Core.Contacts.Models;
using Core.Contacts.Requests;
using Desktop.Common.Exceptions;
using Desktop.Contacts.Models;
using Desktop.Contacts.Services;

namespace Desktop.Tests.Contacts;

public class SyncServiceTests
{
    private readonly SyncService _syncService;
    private readonly Mock<IContactBookService> _api;

    public SyncServiceTests()
    {
        _api = new Mock<IContactBookService>();
        _api.Setup(api => api.AddContact(It.IsAny<AddContactRequest>())).Returns(Task.FromResult(It.IsAny<ContactData>()));
        _api.Setup(api => api.UpdateContact(It.IsAny<UpdateContactRequest>())).Returns(Task.FromResult(It.IsAny<ContactData>()));
        _api.Setup(api => api.DeleteContact(It.IsAny<DeleteContactRequest>())).Returns(Task.CompletedTask);
        _syncService = new SyncService(_api.Object);
    }

    [Fact]
    public async Task PushSuccessfulTest()
    {
        // Arrange     
        DeleteContactRequest deleteRequest = new DeleteContactRequest() { Id = Guid.NewGuid().ToString() };
        ContactUnit syncedContact = new ContactUnit(new ContactData(){Id = Guid.NewGuid().ToString()}, State.Synced);
        ContactUnit newContact = new ContactUnit(new ContactData(){Id = Guid.NewGuid().ToString()}, State.New);
        ContactUnit updatedContact = new ContactUnit(new ContactData(){Id = Guid.NewGuid().ToString()}, State.Changed);
        UnitOfWorkState state = new UnitOfWorkState()
        {
            ExistingUnits = new List<ContactUnit>() { syncedContact, newContact, updatedContact },
            PendingDeleteRequests = new List<DeleteContactRequest>() { deleteRequest }
        };
        ContactData addedContact = new ContactData(){ Id = Guid.NewGuid().ToString() };
        _api.Setup(api => api.AddContact(It.IsAny<AddContactRequest>())).Returns(Task.FromResult(addedContact));

        // Act
        await _syncService.Push(state);

        // Assert
        Assert.True(state.ExistingUnits.Find(unit => unit.Id == syncedContact.Id).State == State.Synced);
        Assert.True(state.ExistingUnits.Find(unit => unit.Id == updatedContact.Id).State == State.Synced);
        Assert.True(newContact.Id == addedContact.Id && newContact.State == State.Synced);
        Assert.Contains(newContact, state.ExistingUnits);
        Assert.Empty(state.PendingDeleteRequests);
        Assert.True(state.IsSynced);
    }

    [Fact]
    public async Task PushIfApiServiceThrowsException()
    {
        // Arrange
        DeleteContactRequest deleteRequest = new DeleteContactRequest() { Id = Guid.NewGuid().ToString() };
        ContactUnit syncedContact = new ContactUnit(new ContactData() { Id = Guid.NewGuid().ToString() }, State.Synced);
        ContactUnit newContact = new ContactUnit(new ContactData() { Id = Guid.NewGuid().ToString() }, State.New);
        ContactUnit updatedContact = new ContactUnit(new ContactData() { Id = Guid.NewGuid().ToString() }, State.Changed);
        UnitOfWorkState state = new UnitOfWorkState()
        {
            ExistingUnits = new List<ContactUnit>() { syncedContact, newContact, updatedContact },
            PendingDeleteRequests = new List<DeleteContactRequest>() { deleteRequest }
        };
        _api.Setup(api => api.UpdateContact(It.IsAny<UpdateContactRequest>())).ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<SyncingWithRemoteRepositoryException>(async () => await _syncService.Push(state));
        Assert.False(state.IsSynced);
    }
}
