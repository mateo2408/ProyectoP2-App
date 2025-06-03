namespace NotasAcademicasApp.Services;

using System.Net.Http.Json;
using Models;

public class MateriaService
{
    private readonly HttpClient _httpClient;

    public MateriaService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000/api/")
        };
    }

    public async Task<List<Materia>> GetMateriasAsync()
    {
        var response = await _httpClient.GetAsync("Materias");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<List<Materia>>() ?? new()
            : new();
    }

    public async Task<bool> CreateMateriaAsync(Materia materia)
    {
        var response = await _httpClient.PostAsJsonAsync("Materias", materia);
        return response.IsSuccessStatusCode;
    }
}