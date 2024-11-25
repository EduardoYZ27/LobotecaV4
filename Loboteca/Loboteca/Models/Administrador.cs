using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = null!;

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = null!;

        [Display(Name = "No. de Empleado")]
        public string NumeroDeEmpleado { get; set; } = null!;

        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaDeInicio { get; set; }

        [Display(Name = "Fecha de Termino")]
        public DateTime FechaDeTermino { get; set; }

        public virtual ICollection<Devolucione> Devoluciones { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
