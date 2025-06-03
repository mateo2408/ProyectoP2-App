using System;
using Microsoft.Maui.Controls;
using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;

namespace NotasAcademicasApp.Views;

public partial class MateriaFormPage : ContentPage
{
    private readonly MateriaService _service = new();
    public MateriaFormPage()
    {
        InitializeComponent();
    }

    private async void OnGuardarMateria(object sender, EventArgs e)
    {
        var materia = new Materia
        {
            Nombre = nombreEntry.Text
        };

        var response = await _service.CreateMateriaAsync(materia);
        if (response)
        {
            await DisplayAlert("Éxito", "Materia guardada.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "No se pudo guardar.", "OK");
        }
    }
}