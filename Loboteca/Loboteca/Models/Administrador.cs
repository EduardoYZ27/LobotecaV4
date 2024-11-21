using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class Administrador
    {
        public Administrador()
        {
            Devoluciones = new HashSet<Devolucione>();
            Prestamos = new HashSet<Prestamo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string ApellidoMaterno { get; set; } = null!;
        public string NumeroDeEmpleado { get; set; } = null!;
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeTermino { get; set; }

        public virtual ICollection<Devolucione> Devoluciones { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
