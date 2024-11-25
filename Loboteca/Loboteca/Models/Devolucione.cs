using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loboteca.Models
{
    public partial class Devolucione
    {
        public Devolucione()
        {
            Sanciones = new HashSet<Sancione>();
        }

        public int Id { get; set; }
        public int? IdAdministrador { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdPrestamo { get; set; }
        public DateTime Fecha { get; set; }
        public string Condiciones { get; set; } = null!;


        [Display(Name = "Administrador")]
        public virtual Administrador? IdAdministradorNavigation { get; set; }

        [Display(Name = "No. de Prestamo")]
        public virtual Prestamo? IdPrestamoNavigation { get; set; }

        [Display(Name = "Usuario")]
        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Sancione> Sanciones { get; set; }
    }
}
