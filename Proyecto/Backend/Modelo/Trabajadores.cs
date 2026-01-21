using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("trabajadores")]
[Index("Dni", Name = "dni_UNIQUE", IsUnique = true)]
[Index("RolIdRol", Name = "fk_trabajadores_rol1_idx")]
public partial class Trabajadores
{
    [Key]
    [Column("idTrabajador")]
    public int IdTrabajador { get; set; }

    [Column("dni")]
    [StringLength(9)]
    public string? Dni { get; set; }

    [Column("nombre")]
    [StringLength(45)]
    public string? Nombre { get; set; }

    [Column("estado")]
    [StringLength(15)]
    public string? Estado { get; set; }

    [Column("apellido1")]
    [StringLength(45)]
    public string? Apellido1 { get; set; }

    [Column("apellido2")]
    [StringLength(45)]
    public string? Apellido2 { get; set; }

    [Column("telefono")]
    public int? Telefono { get; set; }

    [Column("ciudad")]
    [StringLength(100)]
    public string? Ciudad { get; set; }

    [Column("calle")]
    [StringLength(45)]
    public string? Calle { get; set; }

    [Column("cp")]
    [StringLength(45)]
    public string? Cp { get; set; }

    [Column("rol_idRol")]
    public int RolIdRol { get; set; }

    [InverseProperty("TrabajadoresIdTrabajadorNavigation")]
    public virtual ICollection<Nomina> Nominas { get; set; } = new List<Nomina>();

    [ForeignKey("RolIdRol")]
    [InverseProperty("Trabajadores")]
    public virtual Rol RolIdRolNavigation { get; set; } = null!;

    [InverseProperty("TrabajadoresIdTrabajadorNavigation")]
    public virtual ICollection<Trabajo> Trabajos { get; set; } = new List<Trabajo>();
}
