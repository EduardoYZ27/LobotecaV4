using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loboteca.Models
{
    public partial class ELibro
    {
        public ELibro()
        {
            AutorELibros = new HashSet<AutorELibro>();
        }

        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Isbn { get; set; } = null!;

        [Display(Name = "Fecha de Publicación")]
        public DateTime FechaDePublicacion { get; set; }

        [Required(ErrorMessage = "Seleccione un género válido.")]
        public string Genero { get; set; }

        public string Estado { get; set; } = null!;
        [BindNever] // Excluye esta propiedad de la validación inicial
        public string RutaDeImagen { get; set; } = null!;
        public string Archivo { get; set; } = null!;

        [Display(Name = "Fecha de Alta")]
        public DateTime FechaDeAlta { get; set; }
        public int? IdEditorial { get; set; }
        [NotMapped] // Esto asegura que no se mapeará a la base de datos
        public int IdAutor { get; set; }

        public virtual Editorial? IdEditorialNavigation { get; set; }
        public virtual ICollection<AutorELibro> AutorELibros { get; set; }
    }
}
