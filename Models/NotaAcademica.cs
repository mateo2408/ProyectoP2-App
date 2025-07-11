using System;
using SQLite;

namespace NotasAcademicasApp.Models
{
    public class NotaAcademica
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public double Calificacion { get; set; }
        public DateTime FechaEvaluacion { get; set; }
    }
}