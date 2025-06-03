using NotasAcademicasApp.Views;
using Microsoft.Maui.Controls;

namespace NotasAcademicasApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new NotasPage());
    }
}