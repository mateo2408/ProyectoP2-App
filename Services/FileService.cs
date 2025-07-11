using NotasAcademicasApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotasAcademicasApp.Services;

public class FileService
{
    private readonly string _dataFolderPath;

    public FileService()
    {
        // Use the app's data directory instead of trying to access project files
        _dataFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Data");
        Directory.CreateDirectory(_dataFolderPath);
    }

    // Export data to .txt files
    public async Task<bool> ExportEstudiantesToTxtAsync(List<Estudiante> estudiantes)
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "estudiantes.txt");
            var json = JsonSerializer.Serialize(estudiantes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ExportMateriasTotxtAsync(List<Materia> materias)
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "materias.txt");
            var json = JsonSerializer.Serialize(materias, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ExportNotasTotxtAsync(List<NotaAcademica> notas)
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "notas.txt");
            var json = JsonSerializer.Serialize(notas, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Import data from .txt files
    public async Task<List<Estudiante>?> ImportEstudiantesFromTxtAsync()
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "estudiantes.txt");
            if (!File.Exists(filePath))
                return null;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Estudiante>>(json);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Materia>?> ImportMateriasFromTxtAsync()
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "materias.txt");
            if (!File.Exists(filePath))
                return null;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<Materia>>(json);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<NotaAcademica>?> ImportNotasFromTxtAsync()
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "notas.txt");
            if (!File.Exists(filePath))
                return null;

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<NotaAcademica>>(json);
        }
        catch
        {
            return null;
        }
    }

    // Get file paths for manual access
    public string GetEstudiantesFilePath() => Path.Combine(_dataFolderPath, "estudiantes.txt");
    public string GetMateriasFilePath() => Path.Combine(_dataFolderPath, "materias.txt");
    public string GetNotasFilePath() => Path.Combine(_dataFolderPath, "notas.txt");

    // Write custom logs or notes to .txt files
    public async Task<bool> WriteLogAsync(string message)
    {
        try
        {
            var filePath = Path.Combine(_dataFolderPath, "app_log.txt");
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var logEntry = $"[{timestamp}] {message}\n";
            await File.AppendAllTextAsync(filePath, logEntry);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
