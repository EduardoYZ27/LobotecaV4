using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class Inventario
    {
        public Inventario()
        {
            InventarioLibros = new HashSet<InventarioLibro>();
        }

        public int Id { get; set; }
        public int CantidadReal { get; set; }
        public int CantidadSistema { get; set; }
        public string Observaciones { get; set; } = null!;

        public virtual ICollection<InventarioLibro> InventarioLibros { get; set; }
    }
}
