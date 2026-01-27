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
    public virtual ICollection<RolHasPermiso> RolHasPermisos { get; set; } = new List<RolHasPermiso>();

    [InverseProperty("RolIdRolNavigation")]
    public virtual ICollection<Trabajadores> Trabajadores { get; set; } = new List<Trabajadores>();
}
