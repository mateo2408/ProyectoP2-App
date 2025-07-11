using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;
using NotasAcademicasApp.Helpers;

namespace NotasAcademicasApp.ViewModels;

public class MateriaViewModel : INotifyPropertyChanged
{
    private readonly MateriaService _materiaService;
    private readonly FileService _fileService;
    private ObservableCollection<Materia> _materias;
    private Materia _selectedMateria;
    private string _nombre;
    private bool _isLoading;

    public MateriaViewModel(MateriaService materiaService, FileService fileService)
    {
        _materiaService = materiaService;
        _fileService = fileService;
        _materias = new ObservableCollection<Materia>();
        _selectedMateria = new Materia { Nombre = "" };
        _nombre = string.Empty;

        // Fix: Use proper AsyncCommand pattern to fix "Guardar" buttons
        LoadMateriasCommand = new AsyncCommand(LoadMaterias);
        CreateMateriaCommand = new AsyncCommand(CreateMateria, () => !string.IsNullOrWhiteSpace(Nombre));
        UpdateMateriaCommand = new AsyncCommand<Materia>(UpdateMateria);
        DeleteMateriaCommand = new AsyncCommand<Materia>(DeleteMateria);
        ExportToFileCommand = new AsyncCommand(ExportToFile);
        NavigateToAddCommand = new AsyncCommand(NavigateToAdd);
        ViewSubjectNotesCommand = new AsyncCommand<Materia>(ViewSubjectNotes);
        SaveCommand = new AsyncCommand(SaveMateria, () => !string.IsNullOrWhiteSpace(Nombre));
        CancelCommand = new AsyncCommand(Cancel);
    }

    public ObservableCollection<Materia> Materias
    {
        get => _materias;
        set => SetProperty(ref _materias, value);
    }

    public Materia SelectedMateria
    {
        get => _selectedMateria;
        set => SetProperty(ref _selectedMateria, value);
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            SetProperty(ref _nombre, value);
            // Fix: Cast to AsyncCommand for proper CanExecute updates
            ((AsyncCommand)CreateMateriaCommand).RaiseCanExecuteChanged();
            ((AsyncCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoadMateriasCommand { get; }
    public ICommand CreateMateriaCommand { get; }
    public ICommand UpdateMateriaCommand { get; }
    public ICommand DeleteMateriaCommand { get; }
    public ICommand ExportToFileCommand { get; }
    public ICommand NavigateToAddCommand { get; }
    public ICommand ViewSubjectNotesCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private async Task LoadMaterias()
    {
        IsLoading = true;
        try
        {
            var materias = await _materiaService.GetMateriasAsync();
            Materias.Clear();
            foreach (var materia in materias)
            {
                Materias.Add(materia);
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar materias: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task CreateMateria()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        IsLoading = true;
        try
        {
            var nuevaMateria = new Materia { Nombre = Nombre };
            var success = await _materiaService.CreateMateriaAsync(nuevaMateria);
            
            if (success)
            {
                Nombre = string.Empty;
                await LoadMaterias();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Materia creada correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear la materia", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al crear materia: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task UpdateMateria(Materia materia)
    {
        if (materia == null)
            return;

        IsLoading = true;
        try
        {
            var success = await _materiaService.UpdateMateriaAsync(materia.Id, materia);
            
            if (success)
            {
                await LoadMaterias();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Materia actualizada correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la materia", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al actualizar materia: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task DeleteMateria(Materia materia)
    {
        if (materia == null)
            return;

        var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", 
            $"¿Está seguro de eliminar la materia {materia.Nombre}?", "Sí", "No");
        
        if (!confirm)
            return;

        IsLoading = true;
        try
        {
            var success = await _materiaService.DeleteMateriaAsync(materia.Id);
            
            if (success)
            {
                await LoadMaterias();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Materia eliminada correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la materia", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar materia: {ex.Message}", "OK");
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
            var success = await _fileService.ExportMateriasTotxtAsync(Materias.ToList());
            var filePath = _fileService.GetMateriasFilePath();
            
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

    private async Task NavigateToAdd()
    {
        try
        {
            await Shell.Current.GoToAsync("materia-form");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
        }
    }

    private async Task ViewSubjectNotes(Materia materia)
    {
        if (materia == null)
            return;

        try
        {
            await Shell.Current.GoToAsync($"subject-notes?subjectId={materia.Id}&subjectName={materia.Nombre}");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al navegar a notas de la materia: {ex.Message}", "OK");
        }
    }

    private async Task SaveMateria()
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            return;

        IsLoading = true;
        try
        {
            var nuevaMateria = new Materia { Nombre = Nombre };
            var success = await _materiaService.CreateMateriaAsync(nuevaMateria);
            
            if (success)
            {
                Nombre = string.Empty;
                await Application.Current.MainPage.DisplayAlert("Éxito", "Materia creada correctamente", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear la materia", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al crear materia: {ex.Message}", "OK");
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

    public async Task LoadMateriasAsync()
    {
        await LoadMaterias();
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
