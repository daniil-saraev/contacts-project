using Contacts.Data.Context;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Data.Services;

internal class ContactsRepository : IContactsRepository
{
    private readonly ContactsDbContext _dbContext;

    public ContactsRepository(ContactsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Contact entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Contacts.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddRangeAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Contacts.AddRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Contact entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Contacts.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Contacts.RemoveRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Contact>> GetAllAsync(string userId, Func<Contact, bool>? predicate = null, CancellationToken cancellationToken = default)
    {
        return await Task.Factory.StartNew(() =>
        {
            var contacts = _dbContext.Contacts.Where(contact => contact.UserId == userId);
            if (predicate != null)
                return contacts.Where(predicate);
            else
                return contacts.AsEnumerable();
        }, cancellationToken);
    }

    public async Task<Contact?> GetAsync(string id, string userId, CancellationToken cancellationToken = default)
    {
        var contact = await _dbContext.Contacts.FindAsync(id, cancellationToken);
        return contact != null && contact.UserId == userId ? contact : null;
    }

    public async Task UpdateAsync(Contact entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Contacts.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<Contact> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Contacts.UpdateRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}