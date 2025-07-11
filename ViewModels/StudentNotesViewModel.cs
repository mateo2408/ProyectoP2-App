using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;
using NotasAcademicasApp.Helpers;

namespace NotasAcademicasApp.ViewModels;

public class StudentNotesViewModel : INotifyPropertyChanged
{
    private readonly NotaService _notaService;
    private ObservableCollection<NotaAcademica> _studentNotes;
    private int _studentId;
    private string _studentName;
    private bool _isLoading;
    private double _averageGrade;
    private int _studentNotesCount;

    public StudentNotesViewModel(NotaService notaService, MateriaService materiaService, FileService fileService)
    {
        _notaService = notaService;
        _studentNotes = new ObservableCollection<NotaAcademica>();
        _studentName = string.Empty;

        // Fix: Use proper AsyncCommand pattern
        LoadStudentNotesCommand = new AsyncCommand(LoadStudentNotes);
        DeleteNoteCommand = new AsyncCommand<NotaAcademica>(DeleteNote);
        AddNoteCommand = new AsyncCommand(AddNote);
        GoBackCommand = new AsyncCommand(GoBack);
    }

    public ObservableCollection<NotaAcademica> StudentNotes
    {
        get => _studentNotes;
        set => SetProperty(ref _studentNotes, value);
    }

    public int StudentId
    {
        get => _studentId;
        set => SetProperty(ref _studentId, value);
    }

    public string StudentName
    {
        get => _studentName;
        set => SetProperty(ref _studentName, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public double AverageGrade
    {
        get => _averageGrade;
        set => SetProperty(ref _averageGrade, value);
    }

    public int StudentNotesCount
    {
        get => _studentNotesCount;
        set => SetProperty(ref _studentNotesCount, value);
    }

    public ICommand LoadStudentNotesCommand { get; }
    public ICommand DeleteNoteCommand { get; }
    public ICommand AddNoteCommand { get; }
    public ICommand GoBackCommand { get; }

    private async Task LoadStudentNotes()
    {
        IsLoading = true;
        try
        {
            var allNotes = await _notaService.GetNotasAsync();
            var studentNotes = allNotes.Where(n => n.EstudianteId == StudentId).ToList();
            
            StudentNotes.Clear();
            foreach (var nota in studentNotes)
            {
                StudentNotes.Add(nota);
            }

            StudentNotesCount = StudentNotes.Count;
            AverageGrade = StudentNotes.Any() ? StudentNotes.Average(n => n.Calificacion) : 0;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al cargar notas: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task DeleteNote(NotaAcademica nota)
    {
        if (nota == null)
            return;

        var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", 
            $"¿Está seguro de eliminar la nota '{nota.Titulo}'?", "Sí", "No");
        
        if (!confirm)
            return;

        IsLoading = true;
        try
        {
            var success = await _notaService.DeleteNotaAsync(nota.Id);
            
            if (success)
            {
                await LoadStudentNotes();
                await Application.Current.MainPage.DisplayAlert("Éxito", "Nota eliminada correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la nota", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al eliminar nota: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddNote()
    {
        try
        {
            await Shell.Current.GoToAsync($"nota-form?studentId={StudentId}&returnToStudent=true");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
        }
    }

    private async Task GoBack()
    {
        try
        {
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Error al regresar: {ex.Message}", "OK");
        }
    }

    public async Task LoadStudentNotesAsync()
    {
        await LoadStudentNotes();
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
