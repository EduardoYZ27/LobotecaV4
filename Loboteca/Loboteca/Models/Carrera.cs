using System;
using System.Collections.Generic;

namespace Loboteca.Models
{
    public partial class Carrera
    {
        public Carrera()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
