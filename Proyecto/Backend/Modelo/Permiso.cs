using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("permisos")]
public partial class Permiso
{
    [Key]
    [Column("idPermiso")]
    public int IdPermiso { get; set; }

    [Column("nombre")]
    [StringLength(20)]
    public string? Nombre { get; set; }

    [ForeignKey("PermisosIdPermiso")]
    [InverseProperty("PermisosIdPermisos")]
    public virtual ICollection<Rol> RolIdRols { get; set; } = new List<Rol>();
}
