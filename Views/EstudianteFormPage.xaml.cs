using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;

namespace NotasAcademicasApp.Views;

public partial class EstudianteFormPage : ContentPage
{
    private readonly EstudianteService _service = new();

    public EstudianteFormPage()
    {
        InitializeComponent();
    }

    private async void OnGuardarEstudiante(object sender, EventArgs e)
    {
        var estudiante = new Estudiante
        {
            Nombre = nombreEntry.Text
        };

        var response = await _service.CreateEstudianteAsync(estudiante);
        if (response)
        {
            await DisplayAlert("Éxito", "Estudiante guardado", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar", "OK");
        }
    }
}