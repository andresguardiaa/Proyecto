using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("factura")]
[Index("ProyectosIdProyecto", Name = "fk_factura_proyectos1_idx")]
public partial class Factura
{
    [Key]
    [Column("idFactura")]
    public int IdFactura { get; set; }

    [Column("fecha", TypeName = "date")]
    public DateTime? Fecha { get; set; }

    [Column("ingreso")]
    public double? Ingreso { get; set; }

    [Column("total")]
    public double? Total { get; set; }

    [Column("iva")]
    public int? Iva { get; set; }

    [Column("proyectos_idProyecto")]
    public int ProyectosIdProyecto { get; set; }

    [ForeignKey("ProyectosIdProyecto")]
    [InverseProperty("Facturas")]
    public virtual Proyecto ProyectosIdProyectoNavigation { get; set; } = null!;
}
