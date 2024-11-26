using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loboteca.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Devoluciones = new HashSet<Devolucione>();
            Prestamos = new HashSet<Prestamo>();
            Sanciones = new HashSet<Sancione>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = null!;

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = null!;
        public string Matricula { get; set; } = null!;
        public int? IdCarrera { get; set; }
        public string Estado { get; set; } = null!;
        public string Contra { get; set; } = null!;

        [Display(Name = "Carrera")]
        public virtual Carrera? IdCarreraNavigation { get; set; }
        public virtual ICollection<Devolucione> Devoluciones { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
        public virtual ICollection<Sancione> Sanciones { get; set; }
    }
}
