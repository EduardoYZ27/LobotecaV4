using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class Sancione
    {
        public int Id { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdPrestamo { get; set; }
        public int? IdDevoluciones { get; set; }
        public double Monto { get; set; }

        public virtual Devolucione? IdDevolucionesNavigation { get; set; }
        public virtual Prestamo? IdPrestamoNavigation { get; set; }
        public virtual Usuario? IdUsuarioNavigation { get; set; }
    }
}
