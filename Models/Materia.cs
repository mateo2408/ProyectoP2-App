using SQLite;

namespace NotasAcademicasApp.Models;

public class Materia
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
}