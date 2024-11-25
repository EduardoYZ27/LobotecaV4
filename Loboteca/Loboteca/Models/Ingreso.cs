using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loboteca.Models
{
    public partial class Ingreso
    {
        public int Id { get; set; }
        public int? IdLibro { get; set; }

        [Display(Name = "Fecha de Ingreso")]
        public DateTime FechaIngreso { get; set; }
        public int Ejemplares { get; set; }

        [Display(Name = "Titulo de Libro")]
        public virtual Libro? IdLibroNavigation { get; set; }
    }
}
