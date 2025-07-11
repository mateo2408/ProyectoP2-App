using System.Collections.Generic;
using System.Threading.Tasks;
using NotasAcademicasApp.Models;

namespace NotasAcademicasApp.Services;

public class MateriaService
{
    private readonly DatabaseService _databaseService;
    private readonly FileService _fileService;

    public MateriaService()
    {
        _databaseService = new DatabaseService();
        _fileService = new FileService();
    }

    public async Task<List<Materia>> GetMateriasAsync()
    {
        var materias = await _databaseService.GetMateriasAsync();
        // Export to .txt file whenever data is retrieved
        await _fileService.ExportMateriasTotxtAsync(materias);
        return materias;
    }

    public async Task<bool> CreateMateriaAsync(Materia materia)
    {
        var result = await _databaseService.CreateMateriaAsync(materia);
        if (result)
        {
            // Update .txt file after successful creation
            var materias = await _databaseService.GetMateriasAsync();
            await _fileService.ExportMateriasTotxtAsync(materias);
            await _fileService.WriteLogAsync($"Materia creada: {materia.Nombre}");
        }
        return result;
    }

    public async Task<bool> UpdateMateriaAsync(int id, Materia materia)
    {
        materia.Id = id;
        var result = await _databaseService.UpdateMateriaAsync(materia);
        if (result)
        {
            // Update .txt file after successful update
            var materias = await _databaseService.GetMateriasAsync();
            await _fileService.ExportMateriasTotxtAsync(materias);
            await _fileService.WriteLogAsync($"Materia actualizada: {materia.Nombre}");
        }
        return result;
    }

    public async Task<bool> DeleteMateriaAsync(int id)
    {
        var result = await _databaseService.DeleteMateriaAsync(id);
        if (result)
        {
            // Update .txt file after successful deletion
            var materias = await _databaseService.GetMateriasAsync();
            await _fileService.ExportMateriasTotxtAsync(materias);
            await _fileService.WriteLogAsync($"Materia eliminada con ID: {id}");
        }
        return result;
    }
}