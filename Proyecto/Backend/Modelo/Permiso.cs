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

    [InverseProperty("PermisosIdPermisoNavigation")]
    public virtual ICollection<RolHasPermiso> RolHasPermisos { get; set; } = new List<RolHasPermiso>();
}
