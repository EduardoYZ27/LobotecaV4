using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class AutorRevistum
    {
        public int Id { get; set; }
        public int IdAutor { get; set; }
        public int IdRevista { get; set; }

        public virtual Autor IdAutorNavigation { get; set; } = null!;
        public virtual Revistum IdRevistaNavigation { get; set; } = null!;
    }
}
