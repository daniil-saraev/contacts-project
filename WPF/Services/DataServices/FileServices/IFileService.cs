﻿using DatabaseApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Services.DataServices.FileServices
{
    public interface IFileService<T>
    {
        public T? Read();

        public void Write(T data);
    }
}
