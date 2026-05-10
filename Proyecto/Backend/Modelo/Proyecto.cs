using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("proyectos")]
[Index("ClienteIdCliente", Name = "fk_proyectos_cliente1_idx")]
public partial class Proyecto
{
    [Key]
    [Column("idProyecto")]
    public int IdProyecto { get; set; }

    [Column("nombre")]
    [StringLength(45)]
    public string? Nombre { get; set; }

    [Column("fechaInicio", TypeName = "date")]
    public DateTime? FechaInicio { get; set; }

    [Column("fechaFin", TypeName = "date")]
    public DateTime? FechaFin { get; set; }

    [Column("horasEstimadas")]
    public int? HorasEstimadas { get; set; }

    [Column("cliente_idCliente")]
    public int ClienteIdCliente { get; set; }

    [ForeignKey("ClienteIdCliente")]
    [InverseProperty("Proyectos")]
    public virtual Cliente ClienteIdClienteNavigation { get; set; } = null!;

    [InverseProperty("ProyectosIdProyectoNavigation")]
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    [InverseProperty("ProyectosIdProyectoNavigation")]
    public virtual ICollection<Trabajo> Trabajos { get; set; } = new List<Trabajo>();
}
