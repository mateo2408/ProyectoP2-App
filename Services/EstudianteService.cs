namespace NotasAcademicasApp.Services;

using System.Net.Http.Json;
using Models;


public class EstudianteService
{
    private readonly HttpClient _httpClient;

    public EstudianteService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5081/api/")
        };
    }

    public async Task<List<Estudiante>> GetEstudiantesAsync()
    {
        var response = await _httpClient.GetAsync("Estudiantes");
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<List<Estudiante>>() ?? new()
            : new();
    }

    public async Task<bool> CreateEstudianteAsync(Estudiante estudiante)
    {
        var response = await _httpClient.PostAsJsonAsync("Estudiantes", estudiante);
        return response.IsSuccessStatusCode;
    }
}