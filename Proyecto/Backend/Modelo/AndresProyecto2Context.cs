using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

public partial class AndresProyecto2Context : DbContext
{
    public AndresProyecto2Context()
    {
    }

    public AndresProyecto2Context(DbContextOptions<AndresProyecto2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Estado> Estados { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<Maquina> Maquinas { get; set; }

    public virtual DbSet<Modelo> Modelos { get; set; }

    public virtual DbSet<Nomina> Nominas { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<RolHasPermiso> RolHasPermisos { get; set; }

    public virtual DbSet<Trabajadore> Trabajadores { get; set; }

    public virtual DbSet<Trabajo> Trabajos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;database=andres_proyecto2;user=root;password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PRIMARY");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.MaquinaIdMaquina).HasName("PRIMARY");

            entity.HasOne(d => d.MaquinaIdMaquinaNavigation).WithOne(p => p.Estado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Estado_maquina1");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.IdFactura).HasName("PRIMARY");

            entity.HasOne(d => d.ProyectosIdProyectoNavigation).WithMany(p => p.Facturas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_factura_proyectos1");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("PRIMARY");
        });

        modelBuilder.Entity<Maquina>(entity =>
        {
            entity.HasKey(e => e.IdMaquina).HasName("PRIMARY");

            entity.HasMany(d => d.GastosIdGastos).WithMany(p => p.MaquinaIdMaquinas)
                .UsingEntity<Dictionary<string, object>>(
                    "GastosHasMaquina",
                    r => r.HasOne<Gasto>().WithMany()
                        .HasForeignKey("GastosIdGasto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_gastos_has_maquina_gastos1"),
                    l => l.HasOne<Maquina>().WithMany()
                        .HasForeignKey("MaquinaIdMaquina")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_gastos_has_maquina_maquina1"),
                    j =>
                    {
                        j.HasKey("MaquinaIdMaquina", "GastosIdGasto").HasName("PRIMARY");
                        j.ToTable("gastos_has_maquina");
                        j.HasIndex(new[] { "GastosIdGasto" }, "fk_gastos_has_maquina_gastos1_idx");
                        j.HasIndex(new[] { "MaquinaIdMaquina" }, "fk_gastos_has_maquina_maquina1_idx");
                        j.IndexerProperty<int>("MaquinaIdMaquina").HasColumnName("maquina_idMaquina");
                        j.IndexerProperty<int>("GastosIdGasto").HasColumnName("gastos_idGasto");
                    });
        });

        modelBuilder.Entity<Modelo>(entity =>
        {
            entity.HasKey(e => e.MaquinaIdMaquina).HasName("PRIMARY");

            entity.HasOne(d => d.MaquinaIdMaquinaNavigation).WithOne(p => p.Modelo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Modelo_maquina1");
        });

        modelBuilder.Entity<Nomina>(entity =>
        {
            entity.HasKey(e => e.NºNomina).HasName("PRIMARY");

            entity.HasOne(d => d.TrabajadoresIdTrabajadorNavigation).WithMany(p => p.Nominas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_nomina_trabajadores1");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PRIMARY");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.IdProyecto).HasName("PRIMARY");

            entity.HasOne(d => d.ClienteIdClienteNavigation).WithMany(p => p.Proyectos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_proyectos_cliente1");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PRIMARY");
        });

        modelBuilder.Entity<RolHasPermiso>(entity =>
        {
            entity.HasKey(e => new { e.PermisosIdPermiso, e.RolIdRol }).HasName("PRIMARY");

            entity.HasOne(d => d.PermisosIdPermisoNavigation).WithMany(p => p.RolHasPermisos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rol_has_permisos_permisos1");

            entity.HasOne(d => d.RolIdRolNavigation).WithMany(p => p.RolHasPermisos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rol_has_permisos_rol1");
        });

        modelBuilder.Entity<Trabajadore>(entity =>
        {
            entity.HasKey(e => e.IdTrabajador).HasName("PRIMARY");

            entity.HasOne(d => d.RolIdRolNavigation).WithMany(p => p.Trabajadores)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trabajadores_rol1");
        });

        modelBuilder.Entity<Trabajo>(entity =>
        {
            entity.HasKey(e => e.IdTrabajo).HasName("PRIMARY");

            entity.HasOne(d => d.MaquinaIdMaquinaNavigation).WithMany(p => p.Trabajos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trabajo_maquina1");

            entity.HasOne(d => d.ProyectosIdProyectoNavigation).WithMany(p => p.Trabajos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trabajo_proyectos1");

            entity.HasOne(d => d.TrabajadoresIdTrabajadorNavigation).WithMany(p => p.Trabajos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trabajo_trabajadores1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
