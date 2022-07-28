using ApiServices;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Desktop.Data
{
    public class TestRepository : IRepository<Contact>
    {
        public HttpClient HttpClient => throw new NotImplementedException();

        private readonly TestDbContext _context;

        public TestRepository(TestDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Contact entity)
        {
            await _context.AddAsync(entity);
        }

        public Task AddRangeAsync(IEnumerable<Contact> entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Contact entity)
        {
            _context.Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(IEnumerable<Contact> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Contact>?> GetAllAsync()
        {
           return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact?> GetAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public Task UpdateAsync(Contact entity)
        {
            _context.Contacts.Update(entity);
            return Task.CompletedTask;
        }

        public Task UpdateRangeAsync(IEnumerable<Contact> entities)
        {
            throw new NotImplementedException();
        }
    }
}
