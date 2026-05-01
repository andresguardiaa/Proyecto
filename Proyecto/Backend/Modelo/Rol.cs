using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("rol")]
public partial class Rol
{
    [Key]
    [Column("idRol")]
    public int IdRol { get; set; }

    [Column("nombre")]
    [StringLength(20)]
    public string? Nombre { get; set; }

    [InverseProperty("RolIdRolNavigation")]
    public virtual ICollection<Trabajadore> Trabajadores { get; set; } = new List<Trabajadore>();

    [ForeignKey("RolIdRol")]
    [InverseProperty("RolIdRols")]
    public virtual ICollection<Permiso> PermisosIdPermisos { get; set; } = new List<Permiso>();
}
