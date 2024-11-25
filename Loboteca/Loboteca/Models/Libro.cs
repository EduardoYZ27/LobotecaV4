using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loboteca.Models
{
    public partial class Libro
    {
        public Libro()
        {
            AutorLibros = new HashSet<AutorLibro>();
            Ingresos = new HashSet<Ingreso>();
            InventarioLibros = new HashSet<InventarioLibro>();
            Prestamos = new HashSet<Prestamo>();
            FechaDeAlta = DateTime.Now;
        }

        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public DateTime FechaDePublicacion { get; set; }
        public string Genero { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public DateTime FechaDeAlta { get; set; }
        public int? IdEditorial { get; set; }
        [BindNever] // Excluye esta propiedad de la validación inicial
        public string RutaDeImagen { get; set; } = null!;
        [NotMapped] // Indica que no debe ser mapeado a la base de datos
        public int IdAutor { get; set; } // Propiedad temporal para el autor

        public virtual Editorial? IdEditorialNavigation { get; set; }
        public virtual ICollection<AutorLibro> AutorLibros { get; set; }
        public virtual ICollection<Ingreso> Ingresos { get; set; }
        public virtual ICollection<InventarioLibro> InventarioLibros { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
