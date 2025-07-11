using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using NotasAcademicasApp.Models;
using NotasAcademicasApp.Services;
using NotasAcademicasApp.Helpers;

namespace NotasAcademicasApp.ViewModels
{
    public class NotaViewModel : INotifyPropertyChanged
    {
        private readonly NotaService _notaService;
        private readonly EstudianteService _estudianteService;
        private readonly MateriaService _materiaService;
        private readonly FileService _fileService;
        private ObservableCollection<NotaAcademica> _notas;
        private ObservableCollection<Estudiante> _estudiantes;
        private ObservableCollection<Materia> _materias;
        private NotaAcademica _selectedNota;
        private bool _isLoading;

        // Form properties
        private string _titulo;
        private string _contenido;
        private DateTime _fecha = DateTime.Now;
        private int _estudianteId;
        private int _materiaId;
        private string _calificacion = "0"; // Fix: Change to string for Entry binding
        private DateTime _fechaEvaluacion = DateTime.Now;
        private Estudiante? _selectedEstudiante;
        private Materia? _selectedMateria;

        public NotaViewModel(NotaService notaService, EstudianteService estudianteService, 
            MateriaService materiaService, FileService fileService)
        {
            _notaService = notaService;
            _estudianteService = estudianteService;
            _materiaService = materiaService;
            _fileService = fileService;
            
            _notas = new ObservableCollection<NotaAcademica>();
            _estudiantes = new ObservableCollection<Estudiante>();
            _materias = new ObservableCollection<Materia>();
            _selectedNota = new NotaAcademica();
            _titulo = string.Empty;
            _contenido = string.Empty;

            // Fix: Use proper AsyncCommand pattern - this will fix the "Guardar" button
            LoadNotasCommand = new AsyncCommand(LoadNotas);
            LoadEstudiantesCommand = new AsyncCommand(LoadEstudiantes);
            LoadMateriasCommand = new AsyncCommand(LoadMaterias);
            CreateNotaCommand = new AsyncCommand(CreateNota, CanCreateNota);
            UpdateNotaCommand = new AsyncCommand<NotaAcademica>(UpdateNota);
            DeleteNotaCommand = new AsyncCommand<NotaAcademica>(DeleteNota);
            ExportToFileCommand = new AsyncCommand(ExportToFile);
            ClearFormCommand = new Command(ClearForm);
            NavigateToAddCommand = new AsyncCommand(NavigateToAdd);
            SaveCommand = new AsyncCommand(SaveNota, CanCreateNota);
            CancelCommand = new AsyncCommand(CancelForm);
        }

        // Properties
        public ObservableCollection<NotaAcademica> Notas
        {
            get => _notas;
            set => SetProperty(ref _notas, value);
        }

        public ObservableCollection<Estudiante> Estudiantes
        {
            get => _estudiantes;
            set => SetProperty(ref _estudiantes, value);
        }

        public ObservableCollection<Materia> Materias
        {
            get => _materias;
            set => SetProperty(ref _materias, value);
        }

        public NotaAcademica SelectedNota
        {
            get => _selectedNota;
            set => SetProperty(ref _selectedNota, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string Titulo
        {
            get => _titulo;
            set
            {
                SetProperty(ref _titulo, value);
                // Fix: Cast to AsyncCommand for proper CanExecute updates
                ((AsyncCommand)CreateNotaCommand).RaiseCanExecuteChanged();
                ((AsyncCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public string Contenido
        {
            get => _contenido;
            set => SetProperty(ref _contenido, value);
        }

        public DateTime Fecha
        {
            get => _fecha;
            set => SetProperty(ref _fecha, value);
        }

        public int EstudianteId
        {
            get => _estudianteId;
            set
            {
                SetProperty(ref _estudianteId, value);
                // Fix: Cast to AsyncCommand for proper CanExecute updates
                ((AsyncCommand)CreateNotaCommand).RaiseCanExecuteChanged();
                ((AsyncCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public int MateriaId
        {
            get => _materiaId;
            set
            {
                SetProperty(ref _materiaId, value);
                // Fix: Cast to AsyncCommand for proper CanExecute updates
                ((AsyncCommand)CreateNotaCommand).RaiseCanExecuteChanged();
                ((AsyncCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public string Calificacion
        {
            get => _calificacion;
            set => SetProperty(ref _calificacion, value);
        }

        public DateTime FechaEvaluacion
        {
            get => _fechaEvaluacion;
            set => SetProperty(ref _fechaEvaluacion, value);
        }

        public Estudiante? SelectedEstudiante
        {
            get => _selectedEstudiante;
            set
            {
                SetProperty(ref _selectedEstudiante, value);
                if (value != null)
                {
                    EstudianteId = value.Id;
                }
            }
        }

        public Materia? SelectedMateria
        {
            get => _selectedMateria;
            set
            {
                SetProperty(ref _selectedMateria, value);
                if (value != null)
                {
                    MateriaId = value.Id;
                }
            }
        }

        // Commands
        public ICommand LoadNotasCommand { get; }
        public ICommand LoadEstudiantesCommand { get; }
        public ICommand LoadMateriasCommand { get; }
        public ICommand CreateNotaCommand { get; }
        public ICommand UpdateNotaCommand { get; }
        public ICommand DeleteNotaCommand { get; }
        public ICommand ExportToFileCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand NavigateToAddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private async Task LoadNotas()
        {
            IsLoading = true;
            try
            {
                var notas = await _notaService.GetNotasAsync();
                Notas.Clear();
                foreach (var nota in notas)
                {
                    Notas.Add(nota);
                }
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

        private async Task LoadEstudiantes()
        {
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
        }

        private async Task LoadMaterias()
        {
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
        }

        private bool CanCreateNota()
        {
            return !string.IsNullOrWhiteSpace(Titulo) && EstudianteId > 0 && MateriaId > 0;
        }

        private async Task CreateNota()
        {
            if (!CanCreateNota())
                return;

            IsLoading = true;
            try
            {
                // Fix: Parse calificacion string to double
                if (!double.TryParse(Calificacion, out double calificacionValue))
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "La calificación debe ser un número válido", "OK");
                    return;
                }

                var nuevaNota = new NotaAcademica
                {
                    Titulo = Titulo,
                    Contenido = Contenido,
                    Fecha = Fecha,
                    EstudianteId = EstudianteId,
                    MateriaId = MateriaId,
                    Calificacion = calificacionValue, // Use parsed double value
                    FechaEvaluacion = FechaEvaluacion
                };

                var success = await _notaService.CreateNotaAsync(nuevaNota);
                
                if (success)
                {
                    ClearForm();
                    await LoadNotas();
                    await Application.Current?.MainPage?.DisplayAlert("Éxito", "Nota creada correctamente", "OK");
                }
                else
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "No se pudo crear la nota", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", $"Error al crear nota: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task UpdateNota(NotaAcademica nota)
        {
            if (nota == null)
                return;

            IsLoading = true;
            try
            {
                var success = await _notaService.UpdateNotaAsync(nota.Id, nota);
                
                if (success)
                {
                    await LoadNotas();
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Nota actualizada correctamente", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo actualizar la nota", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al actualizar nota: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteNota(NotaAcademica nota)
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
                    await LoadNotas();
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

        private async Task ExportToFile()
        {
            try
            {
                var success = await _fileService.ExportNotasTotxtAsync(Notas.ToList());
                var filePath = _fileService.GetNotasFilePath();
                
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

        private void ClearForm()
        {
            Titulo = string.Empty;
            Contenido = string.Empty;
            Fecha = DateTime.Now;
            EstudianteId = 0;
            MateriaId = 0;
            Calificacion = "0";
            FechaEvaluacion = DateTime.Now;
        }

        private async Task NavigateToAdd()
        {
            try
            {
                await Shell.Current.GoToAsync("nota-form");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al navegar: {ex.Message}", "OK");
            }
        }

        private async Task SaveNota()
        {
            if (!CanCreateNota())
                return;

            IsLoading = true;
            try
            {
                // Fix: Parse calificacion string to double
                if (!double.TryParse(Calificacion, out double calificacionValue))
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "La calificación debe ser un número válido", "OK");
                    return;
                }

                var nuevaNota = new NotaAcademica
                {
                    Titulo = Titulo,
                    Contenido = Contenido,
                    Fecha = Fecha,
                    EstudianteId = EstudianteId,
                    MateriaId = MateriaId,
                    Calificacion = calificacionValue, // Use parsed double value
                    FechaEvaluacion = FechaEvaluacion
                };

                var success = await _notaService.CreateNotaAsync(nuevaNota);
                
                if (success)
                {
                    ClearForm();
                    await Application.Current?.MainPage?.DisplayAlert("��xito", "Nota creada correctamente", "OK");
                    await Shell.Current?.GoToAsync("..");
                }
                else
                {
                    await Application.Current?.MainPage?.DisplayAlert("Error", "No se pudo crear la nota", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", $"Error al crear nota: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task CancelForm()
        {
            try
            {
                ClearForm();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error al cancelar: {ex.Message}", "OK");
            }
        }

        public async Task LoadNotasAsync()
        {
            await LoadNotas();
        }

        public async Task LoadEstudiantesAsync()
        {
            await LoadEstudiantes();
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
}
