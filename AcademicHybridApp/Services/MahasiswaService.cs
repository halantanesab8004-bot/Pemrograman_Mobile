using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services;

public class MahasiswaApiException : Exception
{
    public MahasiswaApiException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}

public class MahasiswaService
{
    private readonly HttpClient _httpClient;

    public MahasiswaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<MahasiswaDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var mahasiswa = await _httpClient.GetFromJsonAsync<List<MahasiswaDto>>("api/Mahasiswa", cancellationToken);
        return mahasiswa ?? new List<MahasiswaDto>();
    }

    public async Task<MahasiswaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/Mahasiswa/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<MahasiswaDto>(cancellationToken);
    }

    public async Task<MahasiswaDto> CreateAsync(MahasiswaRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Mahasiswa", request, cancellationToken);
        await EnsureSuccessAsync(response);

        var mahasiswa = await response.Content.ReadFromJsonAsync<MahasiswaDto>(cancellationToken);
        return mahasiswa ?? throw new MahasiswaApiException(HttpStatusCode.InternalServerError, "Respons API tidak valid.");
    }

    public async Task UpdateAsync(int id, MahasiswaRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Mahasiswa/{id}", request, cancellationToken);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Mahasiswa/{id}", cancellationToken);
        await EnsureSuccessAsync(response);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var message = $"Permintaan ke API gagal (HTTP {(int)response.StatusCode}).";

        try
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
            {
                using var document = JsonDocument.Parse(content);
                var root = document.RootElement;

                if (root.TryGetProperty("message", out var messageElement) &&
                    messageElement.ValueKind == JsonValueKind.String)
                {
                    message = messageElement.GetString() ?? message;
                }
                else if (root.TryGetProperty("errors", out var errorsElement) &&
                         errorsElement.ValueKind == JsonValueKind.Object)
                {
                    var validationMessages = new List<string>();
                    foreach (var error in errorsElement.EnumerateObject())
                    {
                        foreach (var value in error.Value.EnumerateArray())
                        {
                            if (value.ValueKind == JsonValueKind.String)
                            {
                                var text = value.GetString();
                                if (!string.IsNullOrWhiteSpace(text))
                                {
                                    validationMessages.Add(text);
                                }
                            }
                        }
                    }

                    if (validationMessages.Count > 0)
                    {
                        message = string.Join(" ", validationMessages);
                    }
                }
                else if (root.TryGetProperty("title", out var titleElement) &&
                         titleElement.ValueKind == JsonValueKind.String)
                {
                    message = titleElement.GetString() ?? message;
                }
            }
        }
        catch (JsonException)
        {
        }

        throw new MahasiswaApiException(response.StatusCode, message);
    }
}
