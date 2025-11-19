using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("cliente")]
public partial class Cliente
{
    [Key]
    [Column("idCliente")]
    public int IdCliente { get; set; }

    [Column("nombre")]
    [StringLength(100)]
    public string? Nombre { get; set; }

    [Column("telefono")]
    [StringLength(9)]
    public string? Telefono { get; set; }

    [Column("correo")]
    [StringLength(100)]
    public string? Correo { get; set; }

    [Column("ciudad")]
    [StringLength(45)]
    public string? Ciudad { get; set; }

    [Column("calle")]
    [StringLength(45)]
    public string? Calle { get; set; }

    [Column("cp")]
    [StringLength(45)]
    public string? Cp { get; set; }

    [InverseProperty("ClienteIdClienteNavigation")]
    public virtual ICollection<Proyecto> Proyectos { get; set; } = new List<Proyecto>();
}
