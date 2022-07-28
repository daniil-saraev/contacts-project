using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Queries
{
    public interface IQuery<T>
    {
        Task<T?> Execute(object? parameter = null);
    }
}
