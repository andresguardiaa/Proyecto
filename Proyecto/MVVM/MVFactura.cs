using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Proyecto.MVVM
{
    public class MVFactura : MVBase
    {
        private Factura _factura;
        private FacturaRepository _facturaRepository;

        private List<Factura> _facturaList;
        private ListCollectionView _listaFacturasFiltro;
        private DateTime? _fechaFiltro;

        public ListCollectionView listaFacturasFiltro
        {
            get => _listaFacturasFiltro;
            set => SetProperty(ref _listaFacturasFiltro, value);
        }

        public DateTime? FechaFacturaFiltro
        {
            get => _fechaFiltro;
            set => SetProperty(ref _fechaFiltro, value);
        }

        public MVFactura(FacturaRepository facturaRepository)
        {
            _facturaRepository = facturaRepository;
            _factura = new Factura();
        }
        public async Task Inicializa()
        {
            _facturaList = await _facturaRepository.GetFacturasConProyectosAsync();
            listaFacturasFiltro = new ListCollectionView(_facturaList);
        }

        private bool PredicadoFiltro(object item)
        {
            if (item is Gasto gasto)
            {
                // Si la fecha filtro es nula, mostramos todo, si no, aplicamos la condición
                if (FechaFacturaFiltro == null) return true;

                return gasto.Fecha >= FechaFacturaFiltro.Value;
            }
            return false;
        }

        public void Filtrar()
        {
            if (listaFacturasFiltro != null)
            {
                listaFacturasFiltro.Filter = PredicadoFiltro;
            }
        }


        public void LimpiarFiltro()
        {
            FechaFacturaFiltro = null;
            if (listaFacturasFiltro != null)
            {
                listaFacturasFiltro.Filter = null;
            }
        }

    }
}
