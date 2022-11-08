using Core.Entities;
using Desktop.Exceptions;
using System;
using System.Threading.Tasks;

namespace Desktop.App.Services.Data.Persistence.DiskProvider
{
    public class DiskProvider : IDiskProvider
    {
        private readonly IFileService<UnitOfWorkState<Contact>> _fileService;

        public DiskProvider(IFileService<UnitOfWorkState<Contact>> fileService)
        {
            _fileService = fileService;
        }

        public Task<UnitOfWorkState<Contact>?> TryLoadFromDiskAsync()
        {
            UnitOfWorkState<Contact>? unitOfWorkState = null;
            return Task.Run(() =>
            {
                try
                {
                    return unitOfWorkState = _fileService.Read();
                }
                catch (Exception)
                {
                    throw new ReadingDataException();
                }
            });
        }

        public Task TrySaveToDiskAsync(UnitOfWorkState<Contact> unitOfWorkState)
        {
            return Task.Run(() =>
            {
                try
                {
                    _fileService.Write(unitOfWorkState);
                }
                catch (Exception)
                {
                    throw new WritingDataException();
                }
            });
        }
    }
}
