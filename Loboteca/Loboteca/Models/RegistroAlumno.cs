using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class RegistroAlumno
    {
        public RegistroAlumno()
        {
            // Inicialización de colecciones
            Devoluciones = new HashSet<Devolucione>();
            Prestamos = new HashSet<Prestamo>();
            Sanciones = new HashSet<Sancione>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? Matricula { get; set; }
        public int? IdCarrera { get; set; }
        public string? Estado { get; set; }
        public string? Contra { get; set; }

        // Relación con la carrera
        public virtual Carrera? IdCarreraNavigation { get; set; }

        // Colecciones de otras entidades
        public virtual ICollection<Devolucione> Devoluciones { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
        public virtual ICollection<Sancione> Sanciones { get; set; }

        // Propiedad calculada para obtener el nombre de la carrera
        public string? NombreCarrera
        {
            get
            {
                return IdCarreraNavigation?.Nombre;  // Accede al nombre de la carrera a través de la relación de navegación
            }
        }
    }
}
