using NotasAcademicasApp.Views;
using NotasAcademicasApp.Services;

namespace NotasAcademicasApp.Views;

public partial class MateriaPage : ContentPage
{
    private readonly MateriaService _service = new();

    public MateriaPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var materias = await _service.GetMateriasAsync();
        MateriasCollectionView.ItemsSource = materias;
    }

    private async void OnAgregarMateria(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MateriaFormPage());
    }
}