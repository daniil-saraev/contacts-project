using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Common.Exceptions
{
	public class DataNotFoundException : Exception
	{
		public DataNotFoundException() : base("Data was not found") { }
		public DataNotFoundException(string message) : base(message) { }
		public DataNotFoundException(Exception inner) : base("Data was not found", inner) { }

	}
}
