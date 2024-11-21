using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class AutorLibro
    {
        public int Id { get; set; }
        public int? IdAutor { get; set; }
        public int? IdLibro { get; set; }

        public virtual Autor? IdAutorNavigation { get; set; }
        public virtual Libro? IdLibroNavigation { get; set; }
    }
}
