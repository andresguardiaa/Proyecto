using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("modelo")]
public partial class Modelo
{
    [Key]
    [Column("idModelo")]
    public int IdModelo { get; set; }

    [Column("modeloMaquina")]
    [StringLength(45)]
    public string ModeloMaquina { get; set; } = null!;

    [InverseProperty("IdModeloNavigation")]
    public virtual ICollection<Maquina> Maquinas { get; set; } = new List<Maquina>();
}
