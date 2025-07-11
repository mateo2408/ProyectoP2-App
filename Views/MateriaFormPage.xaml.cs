using Microsoft.Maui.Controls;
using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

public partial class MateriaFormPage : ContentPage
{
    public MateriaFormPage(MateriaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
