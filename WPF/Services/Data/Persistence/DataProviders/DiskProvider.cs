﻿using Desktop.Exceptions;
using Desktop.Services.Data.FileServices;
using Desktop.Services.Data.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace Desktop.Services.Data.Persistence.DataProviders
{
    public class DiskProvider
    {
        private readonly IFileService<UnitOfWorkState<Contact>> _fileService;

        public DiskProvider(IFileService<UnitOfWorkState<Contact>> fileService)
        {
            _fileService = fileService;
        }

        public Task<UnitOfWorkState<Contact>?> TryLoadFromDisk()
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

        public Task TrySaveToDisk(UnitOfWorkState<Contact> unitOfWorkState)
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