using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("modelo")]
public partial class Modelo
{
    [Column("modeloMaquina")]
    [StringLength(45)]
    public string ModeloMaquina { get; set; } = null!;

    [Key]
    [Column("maquina_idMaquina")]
    public int MaquinaIdMaquina { get; set; }

    [ForeignKey("MaquinaIdMaquina")]
    [InverseProperty("Modelo")]
    public virtual Maquina MaquinaIdMaquinaNavigation { get; set; } = null!;
}
