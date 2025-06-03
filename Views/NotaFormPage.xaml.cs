using Microsoft.Maui.Controls;
using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;

namespace NotasAcademicasApp.Views;

public partial class NotaFormPage : ContentPage
{
    private readonly NotaService _service = new();
    public NotaFormPage()
    {
        InitializeComponent();
    }

    private async void OnGuardarNota(object sender, EventArgs e)
    {
        if (!int.TryParse(estudianteEntry.Text, out int estudianteId) ||
            !int.TryParse(materiaEntry.Text, out int materiaId) ||
            !decimal.TryParse(calificacionEntry.Text, out decimal calificacion))
        {
            await DisplayAlert("Error", "Por favor, ingrese valores válidos.", "OK");
            return;
        }

        var nota = new NotaAcademica
        {
            EstudianteId = estudianteId,
            MateriaId = materiaId,
            Calificacion = calificacion,
            FechaEvaluacion = fechaPicker.Date
        };

        var success = await _service.CreateNotaAsync(nota);
        if (success)
        {
            await DisplayAlert("Éxito", "Nota guardada.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar.", "OK");
        }
    }
}