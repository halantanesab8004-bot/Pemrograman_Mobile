using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AcademicHybridApp.Models;

namespace AcademicHybridApp.Services;

public class DosenApiException : Exception
{
    public DosenApiException(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }
}

public class DosenService
{
    private readonly HttpClient _httpClient;

    public DosenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<DosenDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dosen = await _httpClient.GetFromJsonAsync<List<DosenDto>>("api/Dosen", cancellationToken);
        return dosen ?? new List<DosenDto>();
    }

    public async Task<DosenDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"api/Dosen/{id}", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<DosenDto>(cancellationToken);
    }

    public async Task<DosenDto> CreateAsync(DosenRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Dosen", request, cancellationToken);
        await EnsureSuccessAsync(response);

        var dosen = await response.Content.ReadFromJsonAsync<DosenDto>(cancellationToken);
        return dosen ?? throw new DosenApiException(HttpStatusCode.InternalServerError, "Respons API tidak valid.");
    }

    public async Task UpdateAsync(int id, DosenRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/Dosen/{id}", request, cancellationToken);
        await EnsureSuccessAsync(response);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"api/Dosen/{id}", cancellationToken);
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

        throw new DosenApiException(response.StatusCode, message);
    }
}
