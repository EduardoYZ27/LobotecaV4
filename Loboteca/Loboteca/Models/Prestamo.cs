using System;
using System.Collections.Generic;

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
        public DateTime FechaDePrestamo { get; set; }
        public DateTime FechaDeTermino { get; set; }
        public int? IdAdministrador { get; set; }
        public int? IdLibro { get; set; }
        public int? IdUsuario { get; set; }

        public virtual Administrador? IdAdministradorNavigation { get; set; }
        public virtual Libro? IdLibroNavigation { get; set; }
        public virtual Usuario? IdUsuarioNavigation { get; set; }
        public virtual ICollection<Devolucione> Devoluciones { get; set; }
        public virtual ICollection<Sancione> Sanciones { get; set; }
    }
}
