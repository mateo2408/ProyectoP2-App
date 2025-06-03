namespace NotasAcademicasApp.ViewModels;

using System.Collections.ObjectModel;
using Models;
using Services;

public class NotaViewModel
{
    private readonly NotaService _service = new();

    public ObservableCollection<NotaAcademica> Notas { get; set; } = new();

    public async Task LoadNotasAsync()
    {
        var notas = await _service.GetNotasAsync();
        Notas.Clear();
        foreach (var nota in notas)
            Notas.Add(nota);
    }
}