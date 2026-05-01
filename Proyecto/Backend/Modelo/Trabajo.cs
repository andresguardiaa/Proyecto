using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Backend.Modelo;

[Table("trabajo")]
[Index("MaquinaIdMaquina", Name = "fk_trabajo_maquina1_idx")]
[Index("ProyectosIdProyecto", Name = "fk_trabajo_proyectos1_idx")]
[Index("TrabajadoresIdTrabajador", Name = "fk_trabajo_trabajadores1_idx")]
public partial class Trabajo
{
    [Key]
    [Column("idTrabajo")]
    public int IdTrabajo { get; set; }

    [Column("horasMaquina")]
    public int HorasMaquina { get; set; }

    [Column("Horas_Trabajador")]
    public int HorasTrabajador { get; set; }

    [Column("trabajadores_idTrabajador")]
    public int TrabajadoresIdTrabajador { get; set; }

    [Column("maquina_idMaquina")]
    public int MaquinaIdMaquina { get; set; }

    [Column("proyectos_idProyecto")]
    public int ProyectosIdProyecto { get; set; }

    [ForeignKey("MaquinaIdMaquina")]
    [InverseProperty("Trabajos")]
    public virtual Maquina MaquinaIdMaquinaNavigation { get; set; } = null!;

    [ForeignKey("ProyectosIdProyecto")]
    [InverseProperty("Trabajos")]
    public virtual Proyecto ProyectosIdProyectoNavigation { get; set; } = null!;

    [ForeignKey("TrabajadoresIdTrabajador")]
    [InverseProperty("Trabajos")]
    public virtual Trabajadore TrabajadoresIdTrabajadorNavigation { get; set; } = null!;
}
