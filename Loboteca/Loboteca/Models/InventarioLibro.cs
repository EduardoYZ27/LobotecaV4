using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class InventarioLibro
    {
        public int Id { get; set; }
        public int? IdInventario { get; set; }
        public int? IdLibro { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
        public string InventarioTipo { get; set; } = null!;

        public virtual Inventario? IdInventarioNavigation { get; set; }
        public virtual Libro? IdLibroNavigation { get; set; }
    }
}
