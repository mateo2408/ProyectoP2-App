using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

public partial class MateriaPage : ContentPage
{
    public MateriaPage(MateriaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is MateriaViewModel viewModel)
        {
            // Call the async method directly instead of Command.Execute
            await viewModel.LoadMateriasAsync();
        }
    }
}