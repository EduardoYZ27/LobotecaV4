using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loboteca.Models
{
    public partial class Prestamo
    {
        public Prestamo()
        {
            Devoluciones = new HashSet<Devolucione>();
            Sanciones = new HashSet<Sancione>();
        }

        public int Id { get; set; }

        [Display(Name = "Fecha de Prestamo")]
        public DateTime FechaDePrestamo { get; set; }

        [Display(Name = "Fecha de Termino")]
        public DateTime FechaDeTermino { get; set; }
        public int? IdAdministrador { get; set; }
        public int? IdLibro { get; set; }
        public int? IdUsuario { get; set; }


        [Display(Name = "Administrador")]
        public virtual Administrador? IdAdministradorNavigation { get; set; }

        [Display(Name = "Libro")]
        public virtual Libro? IdLibroNavigation { get; set; }

        [Display(Name = "Usuario")]
        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Devolucione> Devoluciones { get; set; }
        public virtual ICollection<Sancione> Sanciones { get; set; }
    }
}
