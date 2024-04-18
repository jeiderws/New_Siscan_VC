using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Siscan_Vc_DAL.DataContext;

public partial class DbSiscanContext : DbContext
{
    public DbSiscanContext()
    {
    }

    public DbSiscanContext(DbContextOptions<DbSiscanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acudientes> Acudientes { get; set; }

    public virtual DbSet<Aprendiz> Aprendiz { get; set; }

    public virtual DbSet<AreasEmpresa> AreasEmpresas { get; set; }

    public virtual DbSet<AsignacionArea> AsignacionAreas { get; set; }

    public virtual DbSet<AsignacionFicha> AsignacionFichas { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<Coformador> Coformadors { get; set; }

    public virtual DbSet<ConvocatoriaTyt> ConvocatoriaTyts { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<EstadoAprendiz> EstadoAprendizs { get; set; }

    public virtual DbSet<EstadoInscripcionTyt> EstadoInscripcionTyts { get; set; }

    public virtual DbSet<EstadoPrograma> EstadoProgramas { get; set; }

    public virtual DbSet<Ficha> Fichas { get; set; }

    public virtual DbSet<InscripcionTyt> InscripcionTyts { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Modalidad> Modalidads { get; set; }

    public virtual DbSet<NivelPrograma> NivelProgramas { get; set; }

    public virtual DbSet<Notificaciones> Notificaciones { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<Programas> Programas { get; set; }

    public virtual DbSet<Sedes> Sedes { get; set; }

    public virtual DbSet<SeguimientoInstructorAprendiz> SeguimientoInstructorAprendizs { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acudientes>(entity =>
        {
            entity.HasKey(e => e.IdAcudiente).HasName("PK_Acudiente");

            entity.Property(e => e.ApellidoAcudiente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CelularAcudiente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoAcudiente)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreAcudiente)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Aprendiz>(entity =>
        {
            entity.HasKey(e => e.NumeroDocumentoAprendiz);

            entity.ToTable("Aprendiz");

            entity.Property(e => e.NumeroDocumentoAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CelAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DireccionAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.FichaNavigation).WithMany(p => p.Aprendiz)
                .HasForeignKey(d => d.Ficha)
                .HasConstraintName("FK_Aprendiz_Ficha");

            entity.HasOne(d => d.IdAcudientesNavigation).WithMany(p => p.Aprendizs)
                .HasForeignKey(d => d.IdAcudientes)
                .HasConstraintName("FK_Aprendiz_Acudientes");

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Aprendiz)
                .HasForeignKey(d => d.IdCiudad)
                .HasConstraintName("FK_Aprendiz_Ciudad");

            entity.HasOne(d => d.IdEstadoAprendizNavigation).WithMany(p => p.Aprendiz)
                .HasForeignKey(d => d.IdEstadoAprendiz)
                .HasConstraintName("FK_Aprendiz_Estado");

            entity.HasOne(d => d.IdEstadoTytNavigation).WithMany(p => p.Aprendiz)
                .HasForeignKey(d => d.IdEstadoTyt)
                .HasConstraintName("FK_Aprendiz_EstadoInscripcionTYT");

            entity.HasOne(d => d.IdTipodocumentoNavigation).WithMany(p => p.Aprendiz)
                .HasForeignKey(d => d.IdTipodocumento)
                .HasConstraintName("FK_Aprendiz_TipoDocumento");
        });

        modelBuilder.Entity<AreasEmpresa>(entity =>
        {
            entity.HasKey(e => e.IdArea);

            entity.ToTable("AreasEmpresa");

            entity.Property(e => e.DescripcionArea)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreArea)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AsignacionArea>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionArea);

            entity.ToTable("AsignacionArea");

            entity.Property(e => e.NitEmpresa)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAreaNavigation).WithMany(p => p.AsignacionAreas)
                .HasForeignKey(d => d.IdArea)
                .HasConstraintName("FK_AsignacionArea_AreasEmpresa");

            entity.HasOne(d => d.NitEmpresaNavigation).WithMany(p => p.AsignacionAreas)
                .HasForeignKey(d => d.NitEmpresa)
                .HasConstraintName("FK_AsignacionArea_Empresa");
        });

        modelBuilder.Entity<AsignacionFicha>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AsignacionFicha");

            entity.Property(e => e.NumeroDocumentoInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.FichaNavigation).WithMany()
                .HasForeignKey(d => d.Ficha)
                .HasConstraintName("FK_AsignacionFicha_Ficha");

            entity.HasOne(d => d.NumeroDocumentoInstructorNavigation).WithMany()
                .HasForeignKey(d => d.NumeroDocumentoInstructor)
                .HasConstraintName("FK_AsignacionFicha_Instructor");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.IdCiudad);

            entity.ToTable("Ciudad");

            entity.Property(e => e.NombreCiudad)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Ciudad)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudad_Departamento");
        });

        modelBuilder.Entity<Coformador>(entity =>
        {
            entity.HasKey(e => e.IdCoformador);

            entity.ToTable("Coformador");

            entity.Property(e => e.ApellidoCoformador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CelCoformador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoCoformador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NitEmpresa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCoformador)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumentoCoformador)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.NitEmpresaNavigation).WithMany(p => p.Coformadors)
                .HasForeignKey(d => d.NitEmpresa)
                .HasConstraintName("FK_Coformador_Empresa");
        });

        modelBuilder.Entity<ConvocatoriaTyt>(entity =>
        {
            entity.HasKey(e => e.IdConvocatoria);

            entity.ToTable("ConvocatoriaTYT");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento);

            entity.ToTable("Departamento");

            entity.Property(e => e.NombreDepartamento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.IdPais)
                .HasConstraintName("FK_Departamento_Pais");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Nitmpresa);

            entity.ToTable("Empresa");

            entity.Property(e => e.Nitmpresa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DireccionEmpresa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreEmpresa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RepresentanteLegal)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TelefonoEmpresa)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.IdCiudad)
                .HasConstraintName("FK_Empresa_Ciudad");
        });

        modelBuilder.Entity<EstadoAprendiz>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK_Estado");

            entity.ToTable("EstadoAprendiz");

            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoInscripcionTyt>(entity =>
        {
            entity.HasKey(e => e.IdEstadotyt).HasName("PK_Estado_Inscripcion_T&T");

            entity.ToTable("EstadoInscripcionTYT");

            entity.Property(e => e.DescripcionEstadotyt)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadoPrograma>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPrograma);

            entity.ToTable("EstadoPrograma");

            entity.Property(e => e.DescripcionEstadoPrograma)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ficha>(entity =>
        {
            entity.HasKey(e => e.Ficha1);

            entity.ToTable("Ficha");

            entity.Property(e => e.Ficha1)
                .ValueGeneratedNever()
                .HasColumnName("Ficha");
            entity.Property(e => e.CodigoPrograma)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumentoInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Version)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Fichas)
                .HasForeignKey(d => d.IdSede)
                .HasConstraintName("FK_Ficha_Sedes");

            entity.HasOne(d => d.NumeroDocumentoInstructorNavigation).WithMany(p => p.Fichas)
                .HasForeignKey(d => d.NumeroDocumentoInstructor)
                .HasConstraintName("FK_Ficha_Instructor");

            entity.HasOne(d => d.Programa).WithMany(p => p.Fichas)
                .HasForeignKey(d => new { d.CodigoPrograma, d.Version })
                .HasConstraintName("FK_Ficha_Programa");
        });

        modelBuilder.Entity<InscripcionTyt>(entity =>
        {
            entity.HasKey(e => e.CodigoInscripcion).HasName("PK_Inscripcion_T&T");

            entity.ToTable("InscripcionTYT");

            entity.Property(e => e.CodigoInscripcion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdConvocatoriaNavigation).WithMany(p => p.InscripcionTyts)
                .HasForeignKey(d => d.IdConvocatoria)
                .HasConstraintName("FK_InscripcionTYT_ConvocatoriaTYT");

            entity.HasOne(d => d.IdEstadotytNavigation).WithMany(p => p.InscripcionTyts)
                .HasForeignKey(d => d.IdEstadotyt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inscripcion_T&T_Estado_Inscripcion_T&T");

            entity.HasOne(d => d.IdciudadNavigation).WithMany(p => p.InscripcionTyts)
                .HasForeignKey(d => d.Idciudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inscripcion_T&T_Ciudad");
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.NumeroDocumentoInstructor);

            entity.ToTable("Instructor");

            entity.Property(e => e.NumeroDocumentoInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CelInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CorreoInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdTipodocumentoNavigation).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.IdTipodocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Instructor_TipoDocumento");
        });

        modelBuilder.Entity<Modalidad>(entity =>
        {
            entity.HasKey(e => e.IdModalidad);

            entity.ToTable("Modalidad");

            entity.Property(e => e.NombreModalidad)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NivelPrograma>(entity =>
        {
            entity.HasKey(e => e.IdNivelPrograma).HasName("PK_TipoPrograma");

            entity.ToTable("NivelPrograma");

            entity.Property(e => e.NivelPrograma1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NivelPrograma");
        });

        modelBuilder.Entity<Notificaciones>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion);

            entity.Property(e => e.DescripcionNotificion).IsUnicode(false);

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Estado");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.IdPais);

            entity.Property(e => e.NombrePais)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Programas>(entity =>
        {
            entity.HasKey(e => new { e.CodigoPrograma, e.Version });

            entity.ToTable("Programa");

            entity.Property(e => e.CodigoPrograma)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Version)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombrePrograma)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEstadoProgramaNavigation).WithMany(p => p.Programas)
                .HasForeignKey(d => d.IdEstadoPrograma)
                .HasConstraintName("FK_Programa_EstadoPrograma");

            entity.HasOne(d => d.IdNivelProgramaNavigation).WithMany(p => p.Programas)
                .HasForeignKey(d => d.IdNivelPrograma)
                .HasConstraintName("FK_Programa_TipoPrograma");
        });

        modelBuilder.Entity<Sedes>(entity =>
        {
            entity.HasKey(e => e.IdSede);

            entity.Property(e => e.CelSede)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DireccionSede)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreSede)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Sedes)
                .HasForeignKey(d => d.IdCiudad)
                .HasConstraintName("FK_Sedes_Ciudad");
        });

        modelBuilder.Entity<SeguimientoInstructorAprendiz>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Seguimiento_Instructor_Aprendiz");

            entity.Property(e => e.NumeroDocumentoAprendiz)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumeroDocumentoInstructor)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAreaEmpresaNavigation).WithMany()
                .HasForeignKey(d => d.IdAreaEmpresa)
                .HasConstraintName("FK_Seguimiento_Instructor_Aprendiz_AreasEmpresa");

            entity.HasOne(d => d.IdAsignacionAreaNavigation).WithMany()
                .HasForeignKey(d => d.IdAsignacionArea)
                .HasConstraintName("FK_Seguimiento_Instructor_Aprendiz_AsignacionArea");

            entity.HasOne(d => d.IdCoformadorNavigation).WithMany()
                .HasForeignKey(d => d.IdCoformador)
                .HasConstraintName("FK_Seguimiento_Instructor_Aprendiz_Coformador");

            entity.HasOne(d => d.IdModalidadNavigation).WithMany()
                .HasForeignKey(d => d.IdModalidad)
                .HasConstraintName("FK_Seguimiento_Instructor_Aprendiz_Modalidad");

            entity.HasOne(d => d.NumeroDocumentoAprendizNavigation).WithMany()
                .HasForeignKey(d => d.NumeroDocumentoAprendiz)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seguimiento_Instructor_Aprendiz_Aprendiz");

            entity.HasOne(d => d.NumeroDocumentoInstructorNavigation).WithMany()
                .HasForeignKey(d => d.NumeroDocumentoInstructor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seguimiento_Instructor_Aprendiz_Instructor");
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento);

            entity.ToTable("TipoDocumento");

            entity.Property(e => e.TipoDocumento1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TipoDocumento");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
