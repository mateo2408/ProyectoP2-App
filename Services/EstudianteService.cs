using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NotasAcademicasApp.Models;

namespace NotasAcademicasApp.Services;

public class EstudianteService
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5201/api/") };

    public async Task<List<Estudiante>> GetEstudiantesAsync()
        => await _client.GetFromJsonAsync<List<Estudiante>>("Estudiantes") ?? new();

    public async Task<bool> CreateEstudianteAsync(Estudiante estudiante)
    {
        var response = await _client.PostAsJsonAsync("Estudiantes", estudiante);
        return response.IsSuccessStatusCode;
    }
}