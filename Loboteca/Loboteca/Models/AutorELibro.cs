using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class AutorELibro
    {
        public int Id { get; set; }
        public int IdAutor { get; set; }
        public int IdELibro { get; set; }

        public virtual Autor IdAutorNavigation { get; set; } = null!;
        public virtual ELibro IdELibroNavigation { get; set; } = null!;
    }
}
