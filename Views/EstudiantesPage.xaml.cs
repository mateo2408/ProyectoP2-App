using Microsoft.Maui.Controls;
using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

public partial class EstudiantesPage : ContentPage
{
    public EstudiantesPage(EstudianteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EstudianteViewModel viewModel)
        {
            // Fix: Call the async method directly instead of Command.Execute
            await viewModel.LoadEstudiantesAsync();
        }
    }
}