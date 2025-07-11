using System;
using Microsoft.Maui.Controls;
using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

public partial class EstudianteFormPage : ContentPage
{
    public EstudianteFormPage(EstudianteViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}