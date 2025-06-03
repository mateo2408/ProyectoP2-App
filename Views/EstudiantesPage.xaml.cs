using NotasAcademicasApp.Services;

namespace NotasAcademicasApp.Views;

public partial class EstudiantesPage : ContentPage
{
    private readonly EstudianteService _service = new();

    public EstudiantesPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var estudiantes = await _service.GetEstudiantesAsync();
        if (EstudiantesCollectionView != null)
        {
            EstudiantesCollectionView.ItemsSource = estudiantes;
        }
    }

    private async void OnAgregarEstudiante(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EstudianteFormPage());
    }
}