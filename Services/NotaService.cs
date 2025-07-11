using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotasAcademicasApp.Models;

namespace NotasAcademicasApp.Services;

public class NotaService
{
    private readonly DatabaseService _databaseService;
    private readonly FileService _fileService;

    public NotaService()
    {
        _databaseService = new DatabaseService();
        _fileService = new FileService();
    }

    public async Task<List<NotaAcademica>> GetNotasAsync()
    {
        var notas = await _databaseService.GetNotasAsync();
        // Export to .txt file whenever data is retrieved
        await _fileService.ExportNotasTotxtAsync(notas);
        return notas;
    }

    public async Task<bool> CreateNotaAsync(NotaAcademica nota)
    {
        var result = await _databaseService.CreateNotaAsync(nota);
        if (result)
        {
            // Update .txt file after successful creation
            var notas = await _databaseService.GetNotasAsync();
            await _fileService.ExportNotasTotxtAsync(notas);
            await _fileService.WriteLogAsync($"Nota creada: {nota.Titulo} - Calificación: {nota.Calificacion}");
        }
        return result;
    }

    public async Task<bool> UpdateNotaAsync(int id, NotaAcademica nota)
    {
        nota.Id = id;
        var result = await _databaseService.UpdateNotaAsync(nota);
        if (result)
        {
            // Update .txt file after successful update
            var notas = await _databaseService.GetNotasAsync();
            await _fileService.ExportNotasTotxtAsync(notas);
            await _fileService.WriteLogAsync($"Nota actualizada: {nota.Titulo} - Calificación: {nota.Calificacion}");
        }
        return result;
    }

    public async Task<bool> DeleteNotaAsync(int id)
    {
        var result = await _databaseService.DeleteNotaAsync(id);
        if (result)
        {
            // Update .txt file after successful deletion
            var notas = await _databaseService.GetNotasAsync();
            await _fileService.ExportNotasTotxtAsync(notas);
            await _fileService.WriteLogAsync($"Nota eliminada con ID: {id}");
        }
        return result;
    }
}