using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace RaceDay.Web.Services
{
    public class RaceDayApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceDayApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request)
        {
            var context = _httpContextAccessor.HttpContext;

            // Forward the browser's API session cookie to the API.
            if (context != null &&
                context.Request.Headers.TryGetValue("Cookie", out var cookies))
            {
                request.Headers.TryAddWithoutValidation(
                    "Cookie", cookies.ToString());
            }

            var response = await _httpClient.SendAsync(request);

            // Forward cookies returned by the API to the browser.
            if (context != null &&
                response.Headers.TryGetValues("Set-Cookie", out var setCookies))
            {
                foreach (var cookie in setCookies)
                {
                    context.Response.Headers.Append("Set-Cookie", cookie);
                }
            }

            return response;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Get, endpoint);

            using var response = await SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(
            string endpoint, T data)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post, endpoint)
            {
                Content = JsonContent.Create(data)
            };

            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsync<T>(
            string endpoint, T data)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Put, endpoint)
            {
                Content = JsonContent.Create(data)
            };

            return await SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Delete, endpoint);

            return await SendAsync(request);
        }

        // Upload a file through the API to Azure Blob Storage.
        public async Task<HttpResponseMessage> UploadFileAsync(
            string endpoint,
            IFormFile file)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Post, endpoint);

            var fileStream = file.OpenReadStream();

            var content = new StreamContent(fileStream);

            content.Headers.ContentType =
                new MediaTypeHeaderValue(file.ContentType);

            request.Content = content;

            request.Headers.TryAddWithoutValidation(
                "X-File-Name", file.FileName);

            try
            {
                return await SendAsync(request);
            }
            finally
            {
                request.Dispose();
            }
        }
    }
}
