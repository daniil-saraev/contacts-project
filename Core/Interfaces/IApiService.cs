namespace Core.Interfaces
{
    public interface IApiService
    {
        /// <summary>
        /// To set access token.
        /// </summary>
        void InitializeToken(string token);
    }
}
