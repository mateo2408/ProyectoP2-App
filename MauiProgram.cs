using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using NotasAcademicasApp.Services;
using NotasAcademicasApp.ViewModels;

namespace NotasAcademicasApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register services
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<FileService>();
        builder.Services.AddSingleton<EstudianteService>();
        builder.Services.AddSingleton<MateriaService>();
        builder.Services.AddSingleton<NotaService>();

        // Register ViewModels
        builder.Services.AddTransient<NotaViewModel>();
        builder.Services.AddTransient<EstudianteViewModel>();
        builder.Services.AddTransient<MateriaViewModel>();
        builder.Services.AddTransient<StudentNotesViewModel>();

        // Register Pages
        builder.Services.AddTransient<Views.EstudiantesPage>();
        builder.Services.AddTransient<Views.StudentNotesPage>();
        builder.Services.AddTransient<Views.EstudianteFormPage>();
        builder.Services.AddTransient<Views.MateriaPage>();
        builder.Services.AddTransient<Views.MateriaFormPage>();
        builder.Services.AddTransient<Views.NotasPage>();
        builder.Services.AddTransient<Views.NotaFormPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}