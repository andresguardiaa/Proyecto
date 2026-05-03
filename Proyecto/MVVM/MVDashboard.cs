using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System.Collections.ObjectModel;

namespace Proyecto.MVVM
{
    public class MVDashboard : MVBase
    {

        #region miembros privados

        // Repositorios
        private TrabajadorRepository _trabajadorRepository;
        private MaquinaRepository _maquinaRepository;
        private GastoRepository _gaastoRepository;
        private FacturaRepository _facturaRepository;

        //graficos
        private ObservableCollection<ISeries> _seriesProductividad;
        #endregion

        #region miembros públicos

        public ObservableCollection<ISeries> SeriesProductividad
        {
            get => _seriesProductividad;
            set => SetProperty(ref _seriesProductividad, value);
        }
        #endregion


        #region constructor
        public MVDashboard(
            TrabajadorRepository trabajadorRepository, 
            MaquinaRepository maquinaRepository, 
            GastoRepository gastoRepository,
            FacturaRepository facturaRepository
            )
        {
            _trabajadorRepository = trabajadorRepository;
            _maquinaRepository = maquinaRepository;
            _gaastoRepository = gastoRepository;
            _facturaRepository = facturaRepository;

            SeriesProductividad = new ObservableCollection<ISeries>();
            _ = CargarGraficosAsync();
        }
        #endregion

        #region graficos

        private async Task CargarGraficosAsync()
        {
            var maquinas = await _maquinaRepository.GetAllMaquinasAsync();

            var maquinasAgrupadas = maquinas
                .GroupBy(m => m.IdEstadoNavigation != null ? m.IdEstadoNavigation.Descripcion : "Sin Estado asignado")
                .ToList();

            var nuevasSeries = new ObservableCollection<ISeries>();

            foreach (var grupo in maquinasAgrupadas)
            {
                nuevasSeries.Add(new PieSeries<int>
                {
                    Values = new int[] { grupo.Count() }, 
                    Name = grupo.Key 
                });
            }
            SeriesProductividad = nuevasSeries;
        }

        #endregion
    }
}
