using Microsoft.Maui.Controls;
using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp.Views;

[QueryProperty(nameof(StudentId), "studentId")]
[QueryProperty(nameof(StudentName), "studentName")]
public partial class StudentNotesPage : ContentPage
{
    public string? StudentId { get; set; }
    public string? StudentName { get; set; }

    public StudentNotesPage(StudentNotesViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        if (BindingContext is StudentNotesViewModel viewModel)
        {
            // Fix: Add proper error handling for StudentId parsing
            if (int.TryParse(StudentId, out int studentId))
            {
                viewModel.StudentId = studentId;
            }
            else
            {
                viewModel.StudentId = 0;
            }
            
            viewModel.StudentName = StudentName ?? "";
            
            // Load student notes safely
            try
            {
                await viewModel.LoadStudentNotesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error loading student notes: {ex.Message}", "OK");
            }
        }
    }
}
