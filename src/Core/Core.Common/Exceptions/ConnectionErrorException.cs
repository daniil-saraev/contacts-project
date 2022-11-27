namespace Core.Common.Exceptions
{
	public class ConnectionErrorException : Exception
	{
		public ConnectionErrorException() : base("Connection error!") { }
		public ConnectionErrorException(string message) : base(message) { }
		public ConnectionErrorException(Exception inner) : base("Connection error!", inner) { }
	}
}
