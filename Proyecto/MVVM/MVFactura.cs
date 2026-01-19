using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.MVVM
{
    public class MVFactura : MVBase
    {
        private Factura _factura;
        private FacturaRepository _facturaRepository;

        private List<Factura> _facturaList;
        public List<Factura> facturaList => _facturaList;
        public MVFactura(FacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
            _factura = new Factura();
        }
        public async Task Inicializa()
        {
            _facturaList = await _facturaRepository.GetFacturasConProyectosAsync();
        }


    }
}
