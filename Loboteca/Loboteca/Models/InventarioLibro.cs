using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loboteca.Models
{
    public partial class InventarioLibro
    {
        public int Id { get; set; }

        [Display(Name = "Número de Inventario")]
        public int? IdInventario { get; set; }

        [Display(Name = "Libro")]
        public int? IdLibro { get; set; }

        [Display(Name = "Fecha de Apertura")]
        public DateTime FechaApertura { get; set; }

        [Display(Name = "Fecha de Cierre")]
        public DateTime FechaCierre { get; set; }

        [Display(Name = "Tipo de Inventario")]
        public string InventarioTipo { get; set; } = null!;

        public virtual Inventario? IdInventarioNavigation { get; set; }
        public virtual Libro? IdLibroNavigation { get; set; }
    }
}
