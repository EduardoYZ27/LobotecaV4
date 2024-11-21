using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class Revistum
    {
        public Revistum()
        {
            AutorRevista = new HashSet<AutorRevistum>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Issn { get; set; } = null!;
        public DateTime FechaDePublicacion { get; set; }
        public string Genero { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string RutaDeImagen { get; set; } = null!;
        public string Archivo { get; set; } = null!;
        public DateTime FechaDeAlta { get; set; }
        public int? IdEditorial { get; set; }

        public virtual Editorial? IdEditorialNavigation { get; set; }
        public virtual ICollection<AutorRevistum> AutorRevista { get; set; }
    }
}
