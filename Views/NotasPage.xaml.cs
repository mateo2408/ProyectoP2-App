using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

public partial class NotasPage : ContentPage
{
    public NotasPage(NotaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is NotaViewModel viewModel)
        {
            // Properly call the async method directly instead of using Command
            await viewModel.LoadNotasAsync();
        }
    }
}