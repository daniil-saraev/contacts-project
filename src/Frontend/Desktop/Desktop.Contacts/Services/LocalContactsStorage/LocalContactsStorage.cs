using Desktop.Common.Exceptions;
using Desktop.Common.Services;

namespace Desktop.Contacts.Services
{
    internal class LocalContactsStorage : ILocalContactsStorage
    {
        private readonly IFileService<UnitOfWorkState?> _fileService;

        public LocalContactsStorage(IFileService<UnitOfWorkState?> fileService)
        {
            _fileService = fileService;
        }
        
        public async Task<UnitOfWorkState?> Load()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return _fileService.Read();
                }
                catch (Exception)
                {
                    throw new ReadingDataException();
                }
            });
        }

        public Task Save(UnitOfWorkState unitOfWorkState)
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
