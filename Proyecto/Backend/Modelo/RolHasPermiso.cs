using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[PrimaryKey("PermisosIdPermiso", "RolIdRol")]
[Table("rol_has_permisos")]
[Index("PermisosIdPermiso", Name = "fk_rol_has_permisos_permisos1_idx")]
[Index("RolIdRol", Name = "fk_rol_has_permisos_rol1_idx")]
public partial class RolHasPermiso
{
    [Key]
    [Column("permisos_idPermiso")]
    public int PermisosIdPermiso { get; set; }

    [Key]
    [Column("rol_idRol")]
    public int RolIdRol { get; set; }

    /*[Column("usuario")]
    [StringLength(45)]
    public string? Usuario { get; set; }

    [Column("password")]
    [StringLength(45)]
    public string? Password { get; set; }
    */

    [ForeignKey("PermisosIdPermiso")]
    [InverseProperty("RolHasPermisos")]
    public virtual Permiso PermisosIdPermisoNavigation { get; set; } = null!;

    [ForeignKey("RolIdRol")]
    [InverseProperty("RolHasPermisos")]
    public virtual Rol RolIdRolNavigation { get; set; } = null!;
}
