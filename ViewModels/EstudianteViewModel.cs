using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;
using NotasAcademicasApp.Helpers;

namespace NotasAcademicasApp.ViewModels;

public class EstudianteViewModel : INotifyPropertyChanged
{
    private readonly EstudianteService _estudianteService;
    private readonly FileService _fileService;
    private ObservableCollection<Estudiante> _estudiantes;
    private Estudiante _selectedEstudiante;
    private string _nombre;
    private bool _isLoading;

    public EstudianteViewModel(EstudianteService estudianteService, FileService fileService)
    {
        _estudianteService = estudianteService;
        _fileService = fileService;
        _estudiantes = new ObservableCollection<Estudiante>();
        _selectedEstudiante = new Estudiante { Nombre = "" };
        _nombre = string.Empty;

        // Fix: Use proper AsyncCommand pattern to fix "Guardar" buttons
        LoadEstudiantesCommand = new AsyncCommand(LoadEstudiantes);
        CreateEstudianteCommand = new AsyncCommand(CreateEstudiante, () => !string.IsNullOrWhiteSpace(Nombre));
        UpdateEstudianteCommand = new AsyncCommand<Estudiante>(UpdateEstudiante);
        DeleteEstudianteCommand = new AsyncCommand<Estudiante>(DeleteEstudiante);
        ExportToFileCommand = new AsyncCommand(ExportToFile);
        ViewStudentNotesCommand = new AsyncCommand<Estudiante>(ViewStudentNotes);
        SaveCommand = new AsyncCommand(SaveEstudiante, () => !string.IsNullOrWhiteSpace(Nombre));
        CancelCommand = new AsyncCommand(Cancel);
        NavigateToAddCommand = new AsyncCommand(NavigateToAdd);
    }

    public ObservableCollection<Estudiante> Estudiantes
    {
        get => _estudiantes;
        set => SetProperty(ref _estudiantes, value);
    }

    public Estudiante SelectedEstudiante
    {
        get => _selectedEstudiante;
        set => SetProperty(ref _selectedEstudiante, value);
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            SetProperty(ref _nombre, value);
            // Fix: Cast to AsyncCommand for proper CanExecute updates
            ((AsyncCommand)CreateEstudianteCommand).RaiseCanExecuteChanged();
            ((AsyncCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadEstudiantesCommand { get; }
    public ICommand CreateEstudianteCommand { get; }
    public ICommand UpdateEstudianteCommand { get; }
    public ICommand DeleteEstudianteCommand { get; }
    public ICommand ExportToFileCommand { get; }
    public ICommand ViewStudentNotesCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand NavigateToAddCommand { get; }

    private async Task LoadEstudiantes()
    {
        IsLoading = true;
        try
        {
            var estudiantes = await _estudianteService.GetEstudiantesAsync();
            Estudiantes.Clear();
            foreach (var estudiante in estudiantes)
            {
                Estudiantes.Add(estudiante);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar estudiantes: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task CreateEstudiante()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        IsLoading = true;
        try
        {
            var nuevoEstudiante = new Estudiante { Nombre = Nombre };
            var success = await _estudianteService.CreateEstudianteAsync(nuevoEstudiante);
            
            if (success)
            {
                Nombre = string.Empty;
                await LoadEstudiantes();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante creado correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear el estudiante", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al crear estudiante: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task UpdateEstudiante(Estudiante estudiante)
    {
        if (estudiante == null)
            return;

        IsLoading = true;
        try
        {
            var success = await _estudianteService.UpdateEstudianteAsync(estudiante.Id, estudiante);
            
            if (success)
            {
                await LoadEstudiantes();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante actualizado correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar el estudiante", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al actualizar estudiante: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task DeleteEstudiante(Estudiante estudiante)
    {
        if (estudiante == null)
            return;

        var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", 
            $"¿Está seguro de eliminar el estudiante {estudiante.Nombre}?", "Sí", "No");
        
        if (!confirm)
            return;

        IsLoading = true;
        try
        {
            var success = await _estudianteService.DeleteEstudianteAsync(estudiante.Id);
            
            if (success)
            {
                await LoadEstudiantes();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante eliminado correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar el estudiante", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar estudiante: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task ExportToFile()
    {
        try
        {
            var success = await _fileService.ExportEstudiantesToTxtAsync(Estudiantes.ToList());
            var filePath = _fileService.GetEstudiantesFilePath();
            
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", 
                    $"Datos exportados correctamente a:\n{filePath}", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron exportar los datos", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al exportar: {ex.Message}", "OK");
        }
    }

    private async Task ViewStudentNotes(Estudiante estudiante)
    {
        if (estudiante == null)
            return;

        try
        {
            await Shell.Current.GoToAsync($"student-notes?studentId={estudiante.Id}&studentName={estudiante.Nombre}");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al navegar a notas del estudiante: {ex.Message}", "OK");
        }
    }

    private async Task SaveEstudiante()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        IsLoading = true;
        try
        {
            var nuevoEstudiante = new Estudiante { Nombre = Nombre };
            var success = await _estudianteService.CreateEstudianteAsync(nuevoEstudiante);
            
            if (success)
            {
                Nombre = string.Empty;
                await Application.Current.MainPage.DisplayAlert("Éxito", "Estudiante creado correctamente", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear el estudiante", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al crear estudiante: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task Cancel()
    {
        try
        {
            Nombre = string.Empty;
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al cancelar: {ex.Message}", "OK");
        }
    }

    private async Task NavigateToAdd()
    {
        try
        {
            await Shell.Current.GoToAsync("estudiante-form");
        }
        catch (Exception ex)
        {
            // Mejorar el manejo de errores para depuración
            await Application.Current?.MainPage?.DisplayAlert("Error de Navegación", 
                $"No se pudo navegar a la página de agregar estudiante.\nDetalle: {ex.Message}", "OK");
        }
    }

    public async Task LoadEstudiantesAsync()
    {
        await LoadEstudiantes();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
