using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Backend.DTOs
{
    public class ItemGastoUI
    {
        public DateTime? Fecha { get; set; }
        public string Descripcion { get; set; }
        public double? Cantidad { get; set; }
        public string NombreModelo { get; set; }
    }
}
