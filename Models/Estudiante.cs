using SQLite;

namespace NotasAcademicasApp.Models;

public class Estudiante
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}