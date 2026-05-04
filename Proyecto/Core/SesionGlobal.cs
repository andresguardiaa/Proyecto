using Proyecto.Backend.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Core
{
    public static class SesionGlobal
    {
        public static Trabajadore UsuarioActual { get; set; }

        public static bool HaySesionActiva => UsuarioActual != null;

        public static void CerrarSesion()
        {
            UsuarioActual = null;
        }
    }
}
