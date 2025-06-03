namespace NotasAcademicasApp.Services;

using System.Net.Http.Json;
using Models;

public class NotaService
{
    private readonly HttpClient _httpClient;

    public NotaService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://10.0.2.2:5000/api/")
        };
    }

    public async Task<List<NotaAcademica>> GetNotasAsync()
    {
        var response = await _httpClient.GetAsync("NotaAcademica");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<List<NotaAcademica>>() ?? new()
            : new();
    }

    public async Task<bool> CreateNotaAsync(NotaAcademica nota)
    {
        var response = await _httpClient.PostAsJsonAsync("NotaAcademica", nota);
        return response.IsSuccessStatusCode;
    }
}