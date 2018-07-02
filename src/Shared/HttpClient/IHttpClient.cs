namespace Shared
{
    public interface IHttpClient
    {
        T GetBaseHttpTask<T>(string url, string endpoint, params string[] parameters); 
    }
}