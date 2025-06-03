using System;
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
        int.TryParse(estudianteEntry.Text, out int estudianteId);
        int.TryParse(materiaEntry.Text, out int materiaId);
        double.TryParse(calificacionEntry.Text, out double calificacion);
    
        var nota = new NotaAcademica
        {
            Titulo = tituloEntry.Text,
            Contenido = contenidoEntry.Text,
            Fecha = fechaPicker.Date,
            EstudianteId = estudianteId,
            MateriaId = materiaId,
            Calificacion = calificacion,
            FechaEvaluacion = fechaPicker.Date
        };
    
        var response = await _service.CreateNotaAsync(nota);
        if (response)
        {
            await DisplayAlert("Success", "Note added.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Could not add note.", "OK");
        }
    }
}