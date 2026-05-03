using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("gastos")]
public partial class Gasto
{
    [Key]
    [Column("idGasto")]
    public int IdGasto { get; set; }

    [Column("fecha", TypeName = "date")]
    public DateTime? Fecha { get; set; }

    [Column("cantidad")]
    public double? Cantidad { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("tipo")]
    [StringLength(20)]
    public string? Tipo { get; set; }

    [ForeignKey("GastosIdGasto")]
    [InverseProperty("GastosIdGastos")]
    public virtual ICollection<Maquina> MaquinaIdMaquinas { get; set; } = new List<Maquina>();

    [NotMapped] 
    public string NombreModelo
    {
        get
        {
            var maquina = MaquinaIdMaquinas?.FirstOrDefault();

            if (maquina == null)
                return "❌ No hay máquina enlazada";

            if (maquina.IdModeloNavigation == null)
                return "❌ La máquina no tiene modelo";

            return maquina.IdModeloNavigation.ModeloMaquina ?? "❌ El nombre está en blanco";
        }
    }
}
