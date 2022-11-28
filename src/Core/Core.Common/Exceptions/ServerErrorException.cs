namespace Core.Common.Exceptions
{
	public class ServerErrorException : Exception
	{
		public ServerErrorException() : base("Server error!") { }
		public ServerErrorException(string message) : base(message) { }
		public ServerErrorException(Exception inner) : base("Server error!", inner) { }
	}
}
