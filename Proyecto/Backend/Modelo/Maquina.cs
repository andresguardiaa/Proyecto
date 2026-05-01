using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("maquina")]
[Index("IdEstado", Name = "fk_maquina_estado")]
[Index("IdModelo", Name = "fk_maquina_modelo")]
public partial class Maquina
{
    [Key]
    [Column("idMaquina")]
    public int IdMaquina { get; set; }

    [Column("ubicacion")]
    [StringLength(45)]
    public string? Ubicacion { get; set; }

    [Column("tarifa")]
    public int? Tarifa { get; set; }

    [Column("capacidad")]
    public int? Capacidad { get; set; }

    [Column("idModelo")]
    public int? IdModelo { get; set; }

    [Column("idEstado")]
    public int? IdEstado { get; set; }

    [ForeignKey("IdEstado")]
    [InverseProperty("Maquinas")]
    public virtual Estado? IdEstadoNavigation { get; set; }

    [ForeignKey("IdModelo")]
    [InverseProperty("Maquinas")]
    public virtual Modelo? IdModeloNavigation { get; set; }

    [InverseProperty("MaquinaIdMaquinaNavigation")]
    public virtual ICollection<Trabajo> Trabajos { get; set; } = new List<Trabajo>();

    [ForeignKey("MaquinaIdMaquina")]
    [InverseProperty("MaquinaIdMaquinas")]
    public virtual ICollection<Gasto> GastosIdGastos { get; set; } = new List<Gasto>();
}
