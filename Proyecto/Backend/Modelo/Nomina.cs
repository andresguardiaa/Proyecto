using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("nomina")]
[Index("TrabajadoresIdTrabajador", Name = "fk_nomina_trabajadores1_idx")]
public partial class Nomina
{
    [Key]
    [Column("nº_nomina")]
    public int NºNomina { get; set; }

    [Column("fecha", TypeName = "date")]
    public DateTime? Fecha { get; set; }

    [Column("monto")]
    public double? Monto { get; set; }

    [Column("irpf")]
    public int? Irpf { get; set; }

    [Column("trabajadores_idTrabajador")]
    public int TrabajadoresIdTrabajador { get; set; }

    [ForeignKey("TrabajadoresIdTrabajador")]
    [InverseProperty("Nominas")]
    public virtual Trabajadore TrabajadoresIdTrabajadorNavigation { get; set; } = null!;
}
