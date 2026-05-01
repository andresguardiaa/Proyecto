using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("estado")]
public partial class Estado
{
    [Key]
    [Column("idEstado")]
    public int IdEstado { get; set; }

    [Column("descripcion")]
    [StringLength(15)]
    public string? Descripcion { get; set; }

    [InverseProperty("IdEstadoNavigation")]
    public virtual ICollection<Maquina> Maquinas { get; set; } = new List<Maquina>();
}
