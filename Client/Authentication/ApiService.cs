// Services/ApiService.cs
using Microsoft.JSInterop;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Authentication
{
    public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;

    public ApiService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetJwtTokenAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("getJwtToken");
    }

    public async Task<string> CallApiAsync()
    {
        var token = await GetJwtTokenAsync();
        string baseUrl = @"https://app-timesheet-staging-sea.azurewebsites.net/api";

        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{baseUrl}/companies");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }
            else
            {
                throw new HttpRequestException($"API call failed: {response.ReasonPhrase}");
            }
        }
        else
        {
            throw new UnauthorizedAccessException("JWT token not found");
        }
    }
}
}
