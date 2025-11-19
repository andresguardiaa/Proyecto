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
    [Column("maquina_idMaquina")]
    public int MaquinaIdMaquina { get; set; }

    [Column("descripcion")]
    [StringLength(15)]
    public string? Descripcion { get; set; }

    [ForeignKey("MaquinaIdMaquina")]
    [InverseProperty("Estado")]
    public virtual Maquina MaquinaIdMaquinaNavigation { get; set; } = null!;
}
