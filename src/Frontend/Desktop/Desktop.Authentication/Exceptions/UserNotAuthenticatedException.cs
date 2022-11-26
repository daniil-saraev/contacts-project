using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.Authentication.Exceptions
{
	public class UserNotAuthenticatedException : Exception
	{
		public UserNotAuthenticatedException() { }
		public UserNotAuthenticatedException(string message) : base(message) { }
	}
}
