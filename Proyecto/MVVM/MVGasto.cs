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
    public class MVGasto : MVBase
    {
        private Gasto _gasto;
        private GastoRepository _gastoRepository;

        private List<Gasto> _gastoList;
        private ListCollectionView _listaGastosFiltro;
        private DateTime? _fechaFiltro;


        

        public ListCollectionView listaGastosFiltro
        {
            get => _listaGastosFiltro;
            set => SetProperty(ref _listaGastosFiltro, value);
        }
        
        public DateTime? FechaFiltro
        {
            get => _fechaFiltro;
            set => SetProperty(ref _fechaFiltro, value);
        }

        public MVGasto(GastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
            _gasto = new Gasto();
        }

        public async Task Inicializa()
        {
            _gastoList = await GetAllAsync<Gasto>(_gastoRepository);
            listaGastosFiltro = new ListCollectionView(_gastoList);
        }

        private bool PredicadoFiltro(object item)
        {
            if (item is Gasto gasto)
            {
                // Si la fecha filtro es nula, mostramos todo, si no, aplicamos la condición
                if (FechaFiltro == null) return true;

                return gasto.Fecha >= FechaFiltro.Value;
            }
            return false;
        }

        public void Filtrar()
        {
            if(listaGastosFiltro != null)
            {
                listaGastosFiltro.Filter = PredicadoFiltro;
            }
        }


        public void LimpiarFiltro()
        {
            FechaFiltro = null;
            if(listaGastosFiltro != null)
            {
                listaGastosFiltro.Filter = null;
            }
        }

    }
}
