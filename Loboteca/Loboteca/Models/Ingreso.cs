using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class Ingreso
    {
        public int Id { get; set; }
        public int? IdLibro { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int Ejemplares { get; set; }

        public virtual Libro? IdLibroNavigation { get; set; }
    }
}
