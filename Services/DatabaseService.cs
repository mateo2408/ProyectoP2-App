using SQLite;
using NotasAcademicasApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NotasAcademicasApp.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _database;

    public async Task<SQLiteAsyncConnection> GetDatabaseAsync()
    {
        if (_database != null)
            return _database;

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "NotasAcademicas.db");
        _database = new SQLiteAsyncConnection(databasePath);

        // Create tables
        await _database.CreateTableAsync<Estudiante>();
        await _database.CreateTableAsync<Materia>();
        await _database.CreateTableAsync<NotaAcademica>();

        return _database;
    }

    // Estudiante CRUD operations
    public async Task<List<Estudiante>> GetEstudiantesAsync()
    {
        var database = await GetDatabaseAsync();
        return await database.Table<Estudiante>().ToListAsync();
    }

    public async Task<bool> CreateEstudianteAsync(Estudiante estudiante)
    {
        var database = await GetDatabaseAsync();
        var result = await database.InsertAsync(estudiante);
        return result > 0;
    }

    public async Task<bool> UpdateEstudianteAsync(Estudiante estudiante)
    {
        var database = await GetDatabaseAsync();
        var result = await database.UpdateAsync(estudiante);
        return result > 0;
    }

    public async Task<bool> DeleteEstudianteAsync(int id)
    {
        var database = await GetDatabaseAsync();
        var result = await database.DeleteAsync<Estudiante>(id);
        return result > 0;
    }

    // Materia CRUD operations
    public async Task<List<Materia>> GetMateriasAsync()
    {
        var database = await GetDatabaseAsync();
        return await database.Table<Materia>().ToListAsync();
    }

    public async Task<bool> CreateMateriaAsync(Materia materia)
    {
        var database = await GetDatabaseAsync();
        var result = await database.InsertAsync(materia);
        return result > 0;
    }

    public async Task<bool> UpdateMateriaAsync(Materia materia)
    {
        var database = await GetDatabaseAsync();
        var result = await database.UpdateAsync(materia);
        return result > 0;
    }

    public async Task<bool> DeleteMateriaAsync(int id)
    {
        var database = await GetDatabaseAsync();
        var result = await database.DeleteAsync<Materia>(id);
        return result > 0;
    }

    // NotaAcademica CRUD operations
    public async Task<List<NotaAcademica>> GetNotasAsync()
    {
        var database = await GetDatabaseAsync();
        return await database.Table<NotaAcademica>().ToListAsync();
    }

    public async Task<bool> CreateNotaAsync(NotaAcademica nota)
    {
        var database = await GetDatabaseAsync();
        var result = await database.InsertAsync(nota);
        return result > 0;
    }

    public async Task<bool> UpdateNotaAsync(NotaAcademica nota)
    {
        var database = await GetDatabaseAsync();
        var result = await database.UpdateAsync(nota);
        return result > 0;
    }

    public async Task<bool> DeleteNotaAsync(int id)
    {
        var database = await GetDatabaseAsync();
        var result = await database.DeleteAsync<NotaAcademica>(id);
        return result > 0;
    }
}
