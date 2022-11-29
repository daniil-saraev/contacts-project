namespace Core.Common.Exceptions
{
	public class ApiException : Exception
	{
		public ApiException() : base("Error occurred while executing request!") { }
		public ApiException(string message) : base(message) { }
		public ApiException(Exception inner) : base("Error occurred while executing request!", inner) { }
	}
}
