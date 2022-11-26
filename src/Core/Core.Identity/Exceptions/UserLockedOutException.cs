using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Identity.Exceptions;

[Serializable]
public class UserLockedOutException : Exception
{
    public UserLockedOutException() : base("Account locked out") { }

    public UserLockedOutException(string message) : base(message) { }

    protected UserLockedOutException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

}
