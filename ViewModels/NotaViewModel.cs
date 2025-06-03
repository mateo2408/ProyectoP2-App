using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using NotasAcademicasApp.Models;

namespace NotasAcademicasApp.ViewModels
{
    public class NotaViewModel
    {
        public ObservableCollection<NotaAcademica> Notas { get; set; } = new();
        public string ErrorMessage { get; set; }

        public async Task LoadNotasAsync()
        {
            ErrorMessage = null;
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync("http://localhost:5201/api/Notas");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var notas = JsonSerializer.Deserialize<NotaAcademica[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Notas.Clear();
                if (notas != null)
                {
                    foreach (var nota in notas)
                        Notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Could not load notas: " + ex.Message;
            }
        }
    }
}