using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;

namespace Shared
{
    public class HttpClient : IHttpClient
    {
        public T GetBaseHttpTask<T>(string url, string endpoint, params string[] parameters)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentNullException(nameof(endpoint));

            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"{url}/{endpoint}{(parameters != null && parameters.Length > 0 ? "?" + string.Join("&", parameters) : string.Empty)}").Result;
                var result = response.Content.ReadAsStringAsync().Result;

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(result);

                throw new WebException(result);
            }
        }
    }
}
