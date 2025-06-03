using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NotasAcademicasApp.Models;

namespace NotasAcademicasApp.Services;

public class NotaService
{
    private readonly HttpClient _client = new() { BaseAddress = new Uri("http://localhost:5201/api/") };

    public async Task<List<NotaAcademica>> GetNotasAsync()
        => await _client.GetFromJsonAsync<List<NotaAcademica>>("Notas") ?? new();

    public async Task<bool> CreateNotaAsync(NotaAcademica nota)
    {
        var response = await _client.PostAsJsonAsync("Notas", nota);
        return response.IsSuccessStatusCode;
    }
}