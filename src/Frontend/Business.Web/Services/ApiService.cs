namespace Business.Web.Services;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using Business.Web.Models;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppStateService _appState;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ApiService(HttpClient httpClient, AppStateService appState)
    {
        _httpClient = httpClient;
        _appState = appState;
    }

    private void SetAuthHeader()
    {
        if (_appState.CurrentUser != null)
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _appState.CurrentUser.Token);
        else
            _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private ApiResponse<T>? SafeDeserialize<T>(string content)
    {
        if (string.IsNullOrWhiteSpace(content)) return null;
        try { return JsonSerializer.Deserialize<ApiResponse<T>>(content, _jsonOptions); }
        catch { return null; }
    }

    public async Task<ApiResponse<T>?> GetAsync<T>(string endpoint)
    {
        if (!_appState.IsAuthenticated) return null;
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return null;
            var content = await response.Content.ReadAsStringAsync();
            return SafeDeserialize<T>(content);
        }
        catch { return null; }
    }

    public async Task<ApiResponse<T>?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return SafeDeserialize<T>(responseContent);
        }
        catch { return null; }
    }

    public async Task<ApiResponse<T>?> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return SafeDeserialize<T>(responseContent);
        }
        catch { return null; }
    }

    public async Task<ApiResponse<T>?> PatchAsync<T>(string endpoint, object? data = null)
    {
        try
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(data ?? new { });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(endpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return SafeDeserialize<T>(responseContent);
        }
        catch { return null; }
    }

    public async Task<ApiResponse<T>?> DeleteAsync<T>(string endpoint)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();
            return SafeDeserialize<T>(content);
        }
        catch { return null; }
    }

    /// <summary>
    /// Descarga un archivo (p. ej. CSV) protegido por JWT y dispara el guardado en el navegador
    /// vía interop JS (un simple GET con &lt;a href&gt; no llevaría el header Authorization).
    /// </summary>
    public async Task<bool> DownloadFileAsync(IJSRuntime js, string endpoint, string fallbackFileName)
    {
        if (!_appState.IsAuthenticated) return false;
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return false;

            var bytes = await response.Content.ReadAsByteArrayAsync();
            var fileName = response.Content.Headers.ContentDisposition?.FileNameStar
                ?? response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                ?? fallbackFileName;
            var contentType = response.Content.Headers.ContentType?.MediaType ?? "text/csv";

            await js.InvokeVoidAsync("downloadFileFromBytes", Convert.ToBase64String(bytes), fileName, contentType);
            return true;
        }
        catch { return false; }
    }

    public async Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            return SafeDeserialize<LoginResponse>(responseContent);
        }
        catch { return null; }
    }
}
