using Microsoft.Maui.Controls;
using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

[QueryProperty(nameof(StudentId), "studentId")]
[QueryProperty(nameof(ReturnToStudent), "returnToStudent")]
public partial class NotaFormPage : ContentPage
{
    public string? StudentId { get; set; }
    public string? ReturnToStudent { get; set; }

    public NotaFormPage(NotaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is NotaViewModel viewModel)
        {
            // Load students and subjects for dropdowns
            await viewModel.LoadEstudiantesAsync();
            await viewModel.LoadMateriasAsync();
            
            // If coming from student notes page, pre-select the student
            if (!string.IsNullOrEmpty(StudentId) && int.TryParse(StudentId, out int studentId))
            {
                viewModel.EstudianteId = studentId;
                // Find and select the corresponding student object
                await Task.Delay(100); // Small delay to ensure data is loaded
                var student = viewModel.Estudiantes.FirstOrDefault(e => e.Id == studentId);
                if (student != null)
                {
                    viewModel.SelectedEstudiante = student;
                }
            }
        }
    }
}