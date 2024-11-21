using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Loboteca.Models
{
    public partial class LobotecaContext : DbContext
    {
        public LobotecaContext()
        {
        }

        public LobotecaContext(DbContextOptions<LobotecaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrador> Administradors { get; set; } = null!;
        public virtual DbSet<Autor> Autors { get; set; } = null!;
        public virtual DbSet<AutorELibro> AutorELibros { get; set; } = null!;
        public virtual DbSet<AutorLibro> AutorLibros { get; set; } = null!;
        public virtual DbSet<AutorRevistum> AutorRevista { get; set; } = null!;
        public virtual DbSet<Carrera> Carreras { get; set; } = null!;
        public virtual DbSet<Devolucione> Devoluciones { get; set; } = null!;
        public virtual DbSet<ELibro> ELibros { get; set; } = null!;
        public virtual DbSet<Editorial> Editorials { get; set; } = null!;
        public virtual DbSet<Ingreso> Ingresos { get; set; } = null!;
        public virtual DbSet<Inventario> Inventarios { get; set; } = null!;
        public virtual DbSet<InventarioLibro> InventarioLibros { get; set; } = null!;
        public virtual DbSet<Libro> Libros { get; set; } = null!;
        public virtual DbSet<Prestamo> Prestamos { get; set; } = null!;
        public virtual DbSet<Revistum> Revista { get; set; } = null!;
        public virtual DbSet<Sancione> Sanciones { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>(entity =>
            {
                entity.ToTable("administrador");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.FechaDeInicio)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_inicio");

                entity.Property(e => e.FechaDeTermino)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_termino");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NumeroDeEmpleado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("numero_de_empleado");
            });

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.ToTable("autor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<AutorELibro>(entity =>
            {
                entity.ToTable("autor_e_libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAutor).HasColumnName("id_autor");

                entity.Property(e => e.IdELibro).HasColumnName("id_e_libro");

                entity.HasOne(d => d.IdAutorNavigation)
                    .WithMany(p => p.AutorELibros)
                    .HasForeignKey(d => d.IdAutor)
                    .HasConstraintName("FK_autor_e_libro_autor");

                entity.HasOne(d => d.IdELibroNavigation)
                    .WithMany(p => p.AutorELibros)
                    .HasForeignKey(d => d.IdELibro)
                    .HasConstraintName("FK_autor_e_libro_e_libro");
            });

            modelBuilder.Entity<AutorLibro>(entity =>
            {
                entity.ToTable("autor_libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAutor).HasColumnName("id_autor");

                entity.Property(e => e.IdLibro).HasColumnName("id_libro");

                entity.HasOne(d => d.IdAutorNavigation)
                    .WithMany(p => p.AutorLibros)
                    .HasForeignKey(d => d.IdAutor)
                    .HasConstraintName("FK_autor_libro_autor");

                entity.HasOne(d => d.IdLibroNavigation)
                    .WithMany(p => p.AutorLibros)
                    .HasForeignKey(d => d.IdLibro)
                    .HasConstraintName("FK_autor_libro_libro");
            });

            modelBuilder.Entity<AutorRevistum>(entity =>
            {
                entity.ToTable("autor_revista");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdAutor).HasColumnName("id_autor");

                entity.Property(e => e.IdRevista).HasColumnName("id_revista");

                entity.HasOne(d => d.IdAutorNavigation)
                    .WithMany(p => p.AutorRevista)
                    .HasForeignKey(d => d.IdAutor)
                    .HasConstraintName("FK_autor_revista_autor");

                entity.HasOne(d => d.IdRevistaNavigation)
                    .WithMany(p => p.AutorRevista)
                    .HasForeignKey(d => d.IdRevista)
                    .HasConstraintName("FK_autor_revista_revista");
            });

            modelBuilder.Entity<Carrera>(entity =>
            {
                entity.ToTable("carrera");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Devolucione>(entity =>
            {
                entity.ToTable("devoluciones");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Condiciones)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("condiciones");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdAdministrador).HasColumnName("id_administrador");

                entity.Property(e => e.IdPrestamo).HasColumnName("id_prestamo");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.HasOne(d => d.IdAdministradorNavigation)
                    .WithMany(p => p.Devoluciones)
                    .HasForeignKey(d => d.IdAdministrador)
                    .HasConstraintName("FK_devoluciones_administrador");

                entity.HasOne(d => d.IdPrestamoNavigation)
                    .WithMany(p => p.Devoluciones)
                    .HasForeignKey(d => d.IdPrestamo)
                    .HasConstraintName("FK_devoluciones_prestamo");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Devoluciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_devoluciones_usuario");
            });

            modelBuilder.Entity<ELibro>(entity =>
            {
                entity.ToTable("e_libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Archivo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("archivo");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.FechaDeAlta)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_alta");

                entity.Property(e => e.FechaDePublicacion)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_publicacion");

                entity.Property(e => e.Genero)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("genero");

                entity.Property(e => e.IdEditorial).HasColumnName("id_editorial");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.RutaDeImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ruta_de_imagen");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdEditorialNavigation)
                    .WithMany(p => p.ELibros)
                    .HasForeignKey(d => d.IdEditorial)
                    .HasConstraintName("FK_e_libro_editorial");
            });

            modelBuilder.Entity<Editorial>(entity =>
            {
                entity.ToTable("editorial");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Ingreso>(entity =>
            {
                entity.ToTable("ingresos");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ejemplares).HasColumnName("ejemplares");

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("date")
                    .HasColumnName("fecha_ingreso");

                entity.Property(e => e.IdLibro).HasColumnName("id_libro");

                entity.HasOne(d => d.IdLibroNavigation)
                    .WithMany(p => p.Ingresos)
                    .HasForeignKey(d => d.IdLibro)
                    .HasConstraintName("FK_ingresos_libro");
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.ToTable("inventario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CantidadReal).HasColumnName("cantidad_real");

                entity.Property(e => e.CantidadSistema).HasColumnName("cantidad_sistema");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("observaciones");
            });

            modelBuilder.Entity<InventarioLibro>(entity =>
            {
                entity.ToTable("inventario_libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FechaApertura)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_apertura");

                entity.Property(e => e.FechaCierre)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_cierre");

                entity.Property(e => e.IdInventario).HasColumnName("id_inventario");

                entity.Property(e => e.IdLibro).HasColumnName("id_libro");

                entity.Property(e => e.InventarioTipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("inventario_tipo");

                entity.HasOne(d => d.IdInventarioNavigation)
                    .WithMany(p => p.InventarioLibros)
                    .HasForeignKey(d => d.IdInventario)
                    .HasConstraintName("FK_inventario_libro_inventario");

                entity.HasOne(d => d.IdLibroNavigation)
                    .WithMany(p => p.InventarioLibros)
                    .HasForeignKey(d => d.IdLibro)
                    .HasConstraintName("FK_inventario_libro_libro");
            });

            modelBuilder.Entity<Libro>(entity =>
            {
                entity.ToTable("libro");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.FechaDeAlta)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_alta");

                entity.Property(e => e.FechaDePublicacion)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_publicacion");

                entity.Property(e => e.Genero)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("genero");

                entity.Property(e => e.IdEditorial).HasColumnName("id_editorial");

                entity.Property(e => e.Isbn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ISBN");

                entity.Property(e => e.RutaDeImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ruta_de_imagen");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdEditorialNavigation)
                    .WithMany(p => p.Libros)
                    .HasForeignKey(d => d.IdEditorial)
                    .HasConstraintName("FK_libro_editorial");
            });

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("prestamo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FechaDePrestamo)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_prestamo");

                entity.Property(e => e.FechaDeTermino)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_termino");

                entity.Property(e => e.IdAdministrador).HasColumnName("id_administrador");

                entity.Property(e => e.IdLibro).HasColumnName("id_libro");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.HasOne(d => d.IdAdministradorNavigation)
                    .WithMany(p => p.Prestamos)
                    .HasForeignKey(d => d.IdAdministrador)
                    .HasConstraintName("FK_prestamo_administrador");

                entity.HasOne(d => d.IdLibroNavigation)
                    .WithMany(p => p.Prestamos)
                    .HasForeignKey(d => d.IdLibro)
                    .HasConstraintName("FK_prestamo_libro");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Prestamos)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_prestamo_usuario");
            });

            modelBuilder.Entity<Revistum>(entity =>
            {
                entity.ToTable("revista");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Archivo)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("archivo");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.FechaDeAlta)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_alta");

                entity.Property(e => e.FechaDePublicacion)
                    .HasColumnType("date")
                    .HasColumnName("fecha_de_publicacion");

                entity.Property(e => e.Genero)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("genero");

                entity.Property(e => e.IdEditorial).HasColumnName("id_editorial");

                entity.Property(e => e.Issn)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ISSN");

                entity.Property(e => e.RutaDeImagen)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ruta_de_imagen");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdEditorialNavigation)
                    .WithMany(p => p.Revista)
                    .HasForeignKey(d => d.IdEditorial)
                    .HasConstraintName("FK_revista_editorial");
            });

            modelBuilder.Entity<Sancione>(entity =>
            {
                entity.ToTable("sanciones");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdDevoluciones).HasColumnName("id_devoluciones");

                entity.Property(e => e.IdPrestamo).HasColumnName("id_prestamo");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Monto).HasColumnName("monto");

                entity.HasOne(d => d.IdDevolucionesNavigation)
                    .WithMany(p => p.Sanciones)
                    .HasForeignKey(d => d.IdDevoluciones)
                    .HasConstraintName("FK_sanciones_devoluciones");

                entity.HasOne(d => d.IdPrestamoNavigation)
                    .WithMany(p => p.Sanciones)
                    .HasForeignKey(d => d.IdPrestamo)
                    .HasConstraintName("FK_sanciones_prestamo");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Sanciones)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_sanciones_usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_materno");

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("apellido_paterno");

                entity.Property(e => e.Estado)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.IdCarrera).HasColumnName("id_carrera");

                entity.Property(e => e.Matricula)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("matricula");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.HasOne(d => d.IdCarreraNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdCarrera)
                    .HasConstraintName("FK_usuario_carrera");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
