﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loboteca.Models
{
    public partial class Autor
    {
        public Autor()
        {
            AutorELibros = new HashSet<AutorELibro>();
            AutorLibros = new HashSet<AutorLibro>();
            AutorRevista = new HashSet<AutorRevistum>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = null!;

        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = null!;

        public virtual ICollection<AutorELibro> AutorELibros { get; set; }
        public virtual ICollection<AutorLibro> AutorLibros { get; set; }
        public virtual ICollection<AutorRevistum> AutorRevista { get; set; }
    }
}
