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

    public string NombreModelo
    {
        get
        {
            // 1. Accedemos a la lista directa de máquinas
            // IMPORTANTE: Si te da error en ".Maquinas", comprueba en este mismo archivo
            // cómo se llama la lista. A veces EF la llama "MaquinaIdMaquinas".
            var maquina = this.MaquinaIdMaquinas?.FirstOrDefault();

            if (maquina != null && maquina.Modelo != null)
            {
                return maquina.Modelo.ModeloMaquina;
            }

            return "Gasto General";
        }
    }

}
