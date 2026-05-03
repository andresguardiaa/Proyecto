using Proyecto.Backend.DTOs;
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
        private NominaRepository _nominaRepository;

        private List<ItemGastoUI> _gastoList;
        private double _totalGastos;

        //Filtros
        private ListCollectionView _listaGastosFiltro;
        private List<Predicate<ItemGastoUI>> _criterios;
        private Predicate<Object> _predicadoFiltro;

        private Predicate<ItemGastoUI> _criterioNomina;
        private Predicate<ItemGastoUI> _criterioFechaInicio;
        private Predicate<ItemGastoUI> _criterioFechaFin;

        private DateTime? _fechaInicioFiltro;
        private DateTime? _fechaFinFiltro;

        private bool _filtroNominaActivo;

        public double TotalGastos
        {
            get => _totalGastos;
            set => SetProperty(ref _totalGastos, value);
        }

        public ListCollectionView listaGastosFiltro
        {
            get => _listaGastosFiltro;
            set => SetProperty(ref _listaGastosFiltro, value);
        }
        
        public DateTime? FechaInicioFiltro
        {
            get => _fechaInicioFiltro;
            set => SetProperty(ref _fechaInicioFiltro, value);
        }
        public DateTime? FechaFinFiltro
        {
            get => _fechaFinFiltro;
            set => SetProperty(ref _fechaFinFiltro, value);
        }

        public bool filtroNominaActivo
        {
            get => _filtroNominaActivo;
            set
            {
                SetProperty(ref _filtroNominaActivo, value);
                //Filtrar();
            }
        }

        public MVGasto(GastoRepository gastoRepository, NominaRepository nominaRepository)
        {
            _gastoRepository = gastoRepository;
            _gasto = new Gasto();
            _nominaRepository = nominaRepository;
        }

        public async Task Inicializa()
        {
            var gastoList = await _gastoRepository.GetGastosCompletosAsync();
            var nominasList = await _nominaRepository.GetAllAsync();

            _gastoList = new List<ItemGastoUI>();

            foreach (var g in gastoList)
            {
                _gastoList.Add(new ItemGastoUI
                {
                    Fecha = g.Fecha,
                    Descripcion = g.Descripcion,
                    Cantidad = g.Cantidad,
                    NombreModelo = g.NombreModelo 
                });
            }

            if (nominasList != null)
            {
                foreach (var n in nominasList)
                {
                    _gastoList.Add(new ItemGastoUI
                    {
                        Fecha = n.Fecha,
                        Descripcion = $"Nómina - Trabajador ID: {n.TrabajadoresIdTrabajador}",
                        Cantidad = n.Monto,
                        NombreModelo = "-" 
                    });
                }
            }

            _gastoList = _gastoList.OrderByDescending(x => x.Fecha).ToList();
            listaGastosFiltro = new ListCollectionView(_gastoList);
            InicializaCriterios();
            _criterios = new List<Predicate<ItemGastoUI>>();
            _predicadoFiltro = new Predicate<object>(FiltroCriterios);
            CalcularTotal();
        }

        public void Filtrar()
        {
            AddCriterios();
            listaGastosFiltro.Filter = _predicadoFiltro;
            CalcularTotal();
        }
        public void LimpiarFiltros()
        {
            _criterios.Clear();
            listaGastosFiltro.Filter = _predicadoFiltro;
            FechaFinFiltro = null;
            FechaInicioFiltro = null;
            filtroNominaActivo = false;
            CalcularTotal();
        }

        private void InicializaCriterios()
        {
            _criterioNomina = new Predicate<ItemGastoUI>(v => v.Descripcion.StartsWith("Nómina"));
            _criterioFechaInicio = new Predicate<ItemGastoUI>(v => v.Fecha >= FechaInicioFiltro.Value);
            _criterioFechaFin = new Predicate<ItemGastoUI>(v => v.Fecha <= FechaFinFiltro.Value);
        }

        private void AddCriterios()
        {
            _criterios.Clear();
            if (filtroNominaActivo)
            {
                _criterios.Add(_criterioNomina);
            }
            if (FechaInicioFiltro != null)
            {
                _criterios.Add(_criterioFechaInicio);
            }
            if (FechaFinFiltro != null)
            {
                _criterios.Add(_criterioFechaFin);
            }
        }

        private bool FiltroCriterios(object item)
        {
            bool correcto = true;
            ItemGastoUI vuelo = (ItemGastoUI)item;
            if (_criterios != null)
            {
                correcto = _criterios.TrueForAll(x => x(vuelo));
            }
            return correcto;
        }


        private void CalcularTotal()
        {
            if (listaGastosFiltro == null)
            {
                TotalGastos = 0;
                return;
            }

            double total = 0;
            foreach (ItemGastoUI item in listaGastosFiltro)
            {
                total += item.Cantidad ?? 0;
            }

            TotalGastos = total;
        }
    }
}
