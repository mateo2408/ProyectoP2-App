namespace NotasAcademicasApp.Models;

public class NotaAcademica
{
    public int Id { get; set; }
    public int EstudianteId { get; set; }
    public int MateriaId { get; set; }
    public decimal Calificacion { get; set; }
    public DateTime FechaEvaluacion { get; set; }
}