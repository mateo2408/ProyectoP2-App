using Microsoft.Maui.Controls;
using NotasAcademicasApp.Views;

namespace NotasAcademicasApp.Views;

public partial class NotasPage : ContentPage
{
    public NotasPage()
    {
        InitializeComponent();
    }

    private async void OnActualizarNotas(object sender, EventArgs e)
    {
        if (BindingContext is NotasAcademicasApp.ViewModels.NotaViewModel vm)
            await vm.LoadNotasAsync();
    }

    private async void OnAgregarNota(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NotaFormPage());
    }
}