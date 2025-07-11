using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NotasAcademicasApp.Models;

namespace NotasAcademicasApp.Services;

public class EstudianteService
{
    private readonly DatabaseService _databaseService;
    private readonly FileService _fileService;

    public EstudianteService()
    {
        _databaseService = new DatabaseService();
        _fileService = new FileService();
    }

    public async Task<List<Estudiante>> GetEstudiantesAsync()
    {
        var estudiantes = await _databaseService.GetEstudiantesAsync();
        // Export to .txt file whenever data is retrieved
        await _fileService.ExportEstudiantesToTxtAsync(estudiantes);
        return estudiantes;
    }

    public async Task<bool> CreateEstudianteAsync(Estudiante estudiante)
    {
        var result = await _databaseService.CreateEstudianteAsync(estudiante);
        if (result)
        {
            // Update .txt file after successful creation
            var estudiantes = await _databaseService.GetEstudiantesAsync();
            await _fileService.ExportEstudiantesToTxtAsync(estudiantes);
            await _fileService.WriteLogAsync($"Estudiante creado: {estudiante.Nombre}");
        }
        return result;
    }

    public async Task<bool> UpdateEstudianteAsync(int id, Estudiante estudiante)
    {
        estudiante.Id = id;
        var result = await _databaseService.UpdateEstudianteAsync(estudiante);
        if (result)
        {
            // Update .txt file after successful update
            var estudiantes = await _databaseService.GetEstudiantesAsync();
            await _fileService.ExportEstudiantesToTxtAsync(estudiantes);
            await _fileService.WriteLogAsync($"Estudiante actualizado: {estudiante.Nombre}");
        }
        return result;
    }

    public async Task<bool> DeleteEstudianteAsync(int id)
    {
        var result = await _databaseService.DeleteEstudianteAsync(id);
        if (result)
        {
            // Update .txt file after successful deletion
            var estudiantes = await _databaseService.GetEstudiantesAsync();
            await _fileService.ExportEstudiantesToTxtAsync(estudiantes);
            await _fileService.WriteLogAsync($"Estudiante eliminado con ID: {id}");
        }
        return result;
    }
}