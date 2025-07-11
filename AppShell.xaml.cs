using Microsoft.Maui.Controls;
using NotasAcademicasApp.Views;

namespace NotasAcademicasApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("student-notes", typeof(StudentNotesPage));
        Routing.RegisterRoute("estudiante-form", typeof(EstudianteFormPage));
        Routing.RegisterRoute("materia-form", typeof(MateriaFormPage));
        Routing.RegisterRoute("nota-form", typeof(NotaFormPage));
    }
}