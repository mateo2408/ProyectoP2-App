using System;
using Microsoft.Maui.Controls;
using NotasAcademicasApp.Services;

namespace NotasAcademicasApp.Views;

public partial class NotasPage : ContentPage
{
    private readonly NotaService _service = new();

    public NotasPage() => InitializeComponent();

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        NotasCollectionView.ItemsSource = await _service.GetNotasAsync();
    }

    private async void OnAgregarNota(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NotaFormPage());
    }
}