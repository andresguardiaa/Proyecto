using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Proyecto.Backend.Repositorios;
using Proyecto.Core;
using Proyecto.MVVM.Base;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using LiveChartsCore.Drawing;
using LiveChartsCore;
using System.Windows;

namespace Proyecto.MVVM
{
    public class MVDashboard : MVBase
    {

        #region miembros privados

        // Repositorios
        private TrabajadorRepository _trabajadorRepository;
        private MaquinaRepository _maquinaRepository;
        private GastoRepository _gastoRepository;
        private FacturaRepository _facturaRepository;
        private NominaRepository _nominaRepository;
        private TrabajoRepository _trabajoRepository;
        private ProyectoRepository _proyectoRepository;

        //graficos
        private ObservableCollection<ISeries> _seriesProductividad;
        private ObservableCollection<ISeries> _seriesFinanzas;
        private ObservableCollection<ISeries> _seriesProyectos;
        private ObservableCollection<ISeries> _seriesCostoTrabajador;

        private Axis[] _xAxesFinanzas;
        private Axis[] _xAxesProyectos;
        private Axis[] _xAxesCostoTrabajador;

        //Variables
        private String _nombreUsuario;
        private string _horaActual;

        //KPIs
        private string _kpiProyectos;
        private string _kpiBalance;
        private string _kpiPlantillaActiva;
        private string _kpiMaquinasOperativas;

        private DispatcherTimer _timer;
        #endregion

        #region miembros públicos

        //Graficos
        public ObservableCollection<ISeries> SeriesProductividad
        {
            get => _seriesProductividad;
            set => SetProperty(ref _seriesProductividad, value);
        }
        public ObservableCollection<ISeries> SeriesFinanzas
        {
            get => _seriesFinanzas;
            set => SetProperty(ref _seriesFinanzas, value);
        }
        public ObservableCollection<ISeries> SeriesProyectos
        {
            get => _seriesProyectos;
            set => SetProperty(ref _seriesProyectos, value);
        }
        public ObservableCollection<ISeries> SeriesCostoTrabajador
        {
            get => _seriesCostoTrabajador;
            set => SetProperty(ref _seriesCostoTrabajador, value);
        }
        public Axis[] XAxesFinanzas
        {
            get => _xAxesFinanzas;
            set => SetProperty(ref _xAxesFinanzas, value);
        }
        public Axis[] XAxesProyectos
        {
            get => _xAxesProyectos;
            set => SetProperty(ref _xAxesProyectos, value);
        }
        public Axis[] XAxesCostoTrabajador
        {
            get => _xAxesCostoTrabajador;
            set => SetProperty(ref _xAxesCostoTrabajador, value);
        }

        //UI
        public string NombreUsuario
        {
            get => _nombreUsuario;
            set => SetProperty(ref _nombreUsuario, value);
        }

        public string HoraActual
        {
            get => _horaActual;
            set => SetProperty(ref _horaActual, value);
        }
        public string KpiProyectos
        {
            get => _kpiProyectos;
            set => SetProperty(ref _kpiProyectos, value);
        }

        public string KpiBalance
        {
            get => _kpiBalance;
            set => SetProperty(ref _kpiBalance, value);
        }

        public string KpiPlantillaActiva
        {
            get => _kpiPlantillaActiva;
            set => SetProperty(ref _kpiPlantillaActiva, value);
        }

        public string KpiMaquinasOperativas
        {
            get => _kpiMaquinasOperativas;
            set => SetProperty(ref _kpiMaquinasOperativas, value);
        }
        #endregion


        #region constructor
        public MVDashboard(
            TrabajadorRepository trabajadorRepository, 
            MaquinaRepository maquinaRepository, 
            GastoRepository gastoRepository,
            FacturaRepository facturaRepository,
            NominaRepository nominaRepository,
            TrabajoRepository trabajoRepository,
            ProyectoRepository proyectoRepository
            )
        {
            _trabajadorRepository = trabajadorRepository;
            _maquinaRepository = maquinaRepository;
            _gastoRepository = gastoRepository;
            _facturaRepository = facturaRepository;
            _nominaRepository = nominaRepository;
            _trabajoRepository = trabajoRepository;
            _proyectoRepository = proyectoRepository;

            RefrescarSesion();
            IniciarReloj();            

            SeriesProductividad = new ObservableCollection<ISeries>();
        }

        public async Task InicializarDashboardAsync()
        {
            try
            {
                await CargarKpisAsync();
                await CargarGraficosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fallo crítico al cargar el Dashboard:\n{ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region graficos

        private async Task CargarGraficosAsync()
        {
            await GraficoMaquinasActivas();
            await GraficoFinanzas();
            await GraficoProyectos();
            await GraficoCostoPorTrabajador();
        }

        private async Task GraficoMaquinasActivas()
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
        private async Task GraficoFinanzas()
        {
            var facturas = await _facturaRepository.GetAllAsync(); 
            var gastos = await _gastoRepository.GetAllAsync();
            var nominas = await _nominaRepository.GetAllAsync();

            var ultimos6Meses = Enumerable.Range(0, 12)
                .Select(i => DateTime.Now.Date.AddMonths(-i))
                .OrderBy(d => d) 
                .ToList();

            var labelsMeses = ultimos6Meses.Select(d => d.ToString("MMM yyyy")).ToArray();

            var valoresIngresos = new double[12];
            var valoresGastos = new double[12];
            var valoresNominas = new double[12];

            for (int i = 0; i < 12; i++)
            {
                var mes = ultimos6Meses[i].Month;
                var anio = ultimos6Meses[i].Year;
              
                valoresIngresos[i] = facturas
                    .Where(f => f.Fecha.Value.Month == mes && f.Fecha.Value.Year == anio)
                    .Sum(f => (double)(f.Total ?? 0)); 

                valoresGastos[i] = gastos
                    .Where(g => g.Fecha.Value.Month == mes && g.Fecha.Value.Year == anio)
                    .Sum(g => (double)(g.Cantidad ?? 0)); 

                valoresNominas[i] = nominas
                    .Where(n => n.Fecha.Value.Month == mes && n.Fecha.Value.Year == anio)
                    .Sum(n => (double)(n.Monto ?? 0));
            }

            SeriesFinanzas = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = valoresIngresos,
                    Name = "Ingresos",
                    Stroke = new SolidColorPaint(SKColors.SpringGreen) { StrokeThickness = 3 },
                    GeometryFill = new SolidColorPaint(SKColors.SpringGreen),
                    GeometryStroke = new SolidColorPaint(SKColors.SpringGreen)
                },
                new LineSeries<double>
                {
                    Values = valoresGastos,
                    Name = "Gastos Variables",
                    Stroke = new SolidColorPaint(SKColors.Tomato) { StrokeThickness = 3 },
                    GeometryFill = new SolidColorPaint(SKColors.Tomato),
                    GeometryStroke = new SolidColorPaint(SKColors.Tomato)
                },
                new LineSeries<double>
                {
                    Values = valoresNominas,
                    Name = "Gastos Fijos",
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                    GeometryFill = new SolidColorPaint(SKColors.Blue),
                    GeometryStroke = new SolidColorPaint(SKColors.Blue)
                }
            };

            XAxesFinanzas = new Axis[]
            {
                new Axis { Labels = labelsMeses }
            };
        }

        private async Task GraficoProyectos()
        {
            var trabajos = await _trabajoRepository.GetAllAsync();
            var proyectos = await _proyectoRepository.GetAllAsync(); 

            var trabajosAgrupados = trabajos
                .GroupBy(t => t.ProyectosIdProyecto)
                .OrderBy(g => g.Key)
                .ToList();

            int totalProyectos = trabajosAgrupados.Count; 
            var valoresHorasTrabajador = new double[totalProyectos];
            var valoresHorasMaquina = new double[totalProyectos];
            var valoresHorasEstimadas = new double[totalProyectos];
            var labelsProyectos = new string[totalProyectos];

            for (int i = 0; i < totalProyectos; i++)
            {
                var grupo = trabajosAgrupados[i];
                var idProyecto = grupo.Key;

                var proyecto = proyectos.FirstOrDefault(p => p.IdProyecto == idProyecto);

                valoresHorasTrabajador[i] = grupo.Sum(t => t.HorasTrabajador);
                valoresHorasMaquina[i] = grupo.Sum(t => t.HorasMaquina);
                valoresHorasEstimadas[i] = proyecto?.HorasEstimadas ?? 0; 

                labelsProyectos[i] = "Proyecto " + idProyecto;
            }

            var serieTrabajadores = new ColumnSeries<double>
            {
                Values = valoresHorasTrabajador,
                Name = "Horas Trabajadores",
                Fill = new SolidColorPaint(SKColors.DodgerBlue),
                MaxBarWidth = 40
            };

            var serieMaquinas = new ColumnSeries<double>
            {
                Values = valoresHorasMaquina,
                Name = "Horas Máquinas",
                Fill = new SolidColorPaint(SKColors.Orange),
                MaxBarWidth = 40
            };

            var serieEstimadas = new LineSeries<double>
            {
                Values = valoresHorasEstimadas,
                Name = "Presupuesto (Horas Totales)",
                Stroke = new SolidColorPaint(SKColors.Crimson) { StrokeThickness = 4 },
                GeometryFill = new SolidColorPaint(SKColors.Crimson),
                GeometryStroke = new SolidColorPaint(SKColors.Crimson),
                Fill = null 
            };

            serieTrabajadores.PointMeasured += (point) =>
            {
                var visual = point.Context.Visual;
                if (visual == null) return;

                var index = point.Context.Entity.MetaData!.EntityIndex;
                var delay = index / (float)totalProyectos;
                var duration = TimeSpan.FromSeconds(2.5); 

                var animation = new Animation(t => DelayedEase(t, delay), duration);
                visual.SetTransition(animation);
            };

            serieMaquinas.PointMeasured += (point) =>
            {
                var visual = point.Context.Visual;
                if (visual == null) return;

                var index = point.Context.Entity.MetaData!.EntityIndex;
                var delay = index / (float)totalProyectos;
                var duration = TimeSpan.FromSeconds(2.5);

                var animation = new Animation(t => DelayedEase(t, delay), duration);
                visual.SetTransition(animation);
            };
           
            SeriesProyectos = new ObservableCollection<ISeries> { serieTrabajadores, serieMaquinas, serieEstimadas };

            XAxesProyectos = new Axis[]
            {
                new Axis { Labels = labelsProyectos }
            };
        }

        private async Task GraficoCostoPorTrabajador()
        {
            var trabajos = await _trabajoRepository.GetAllAsync();
            var trabajadores = await _trabajadorRepository.GetAllAsync();
            var nominas = await _nominaRepository.GetAllAsync();
            var gastos = await _gastoRepository.GetGastosCompletosAsync();

            var trabajadoresActivos = trabajadores.Where(t => t.Estado == "Activo").ToList();

            var labelsTrabajadores = new string[trabajadoresActivos.Count];
            var costosNomina = new double[trabajadoresActivos.Count];
            var costosUsoMaquina = new double[trabajadoresActivos.Count];

            for (int i = 0; i < trabajadoresActivos.Count; i++)
            {
                var trabajador = trabajadoresActivos[i];
                labelsTrabajadores[i] = trabajador.Nombre; // Ej. "Carlos"

                costosNomina[i] = nominas
                    .Where(n => n.TrabajadoresIdTrabajador == trabajador.IdTrabajador)
                    .Sum(n => (double)(n.Monto ?? 0));

                var trabajosDelEmpleado = trabajos.Where(t => t.TrabajadoresIdTrabajador == trabajador.IdTrabajador).ToList();

                double costoTotalMaquinariaEmpleado = 0;

                foreach (var trabajo in trabajosDelEmpleado)
                {
                    if (trabajo.MaquinaIdMaquina != 0)
                    {
                        var idMaquina = trabajo.MaquinaIdMaquina;
                        var horasUso = trabajo.HorasMaquina;

                        var maquina = await _maquinaRepository.GetByIdAsync(idMaquina);
                        double gastoTotalMaquina = maquina?.GastosIdGastos?.Sum(g => g.Cantidad ?? 0) ?? 0;

                        double horasTotalesMaquina = trabajos
                            .Where(t => t.MaquinaIdMaquina == idMaquina)
                            .Sum(t => t.HorasMaquina);

                        double costoPorHora = horasTotalesMaquina > 0 ? (gastoTotalMaquina / horasTotalesMaquina) : 0;

                        costoTotalMaquinariaEmpleado += (horasUso * costoPorHora);
                    }
                }

                costosUsoMaquina[i] = Math.Round(costoTotalMaquinariaEmpleado, 2);
            }

            SeriesCostoTrabajador = new ObservableCollection<ISeries>
            {
                new StackedColumnSeries<double>
                {
                    Values = costosNomina,
                    Name = "Costo Salarial (Nóminas)",
                    Fill = new SolidColorPaint(SKColors.DodgerBlue)
                },
                new StackedColumnSeries<double>
                {
                    Values = costosUsoMaquina,
                    Name = "Costo Operativo (Uso Máquinas)",
                    Fill = new SolidColorPaint(SKColors.Tomato)
                }
            };

            XAxesCostoTrabajador = new Axis[]
            {
                new Axis { Labels = labelsTrabajadores }
            };
        }
        #endregion

        #region Metodos Animacion LiveCharts

        private static float DelayedEase(float t, float delay)
        {
            if (t <= delay) return 0f;
            var remappedT = (t - delay) / (1f - delay);
            var baseEasing = EasingFunctions.BuildCustomElasticOut(1.5f, 0.60f); 
            return baseEasing(Clamp(remappedT, 0f, 1f));
        }

        private static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        #endregion

        #region usuario y sesión
        public void RefrescarSesion()
        {
            if (SesionGlobal.HaySesionActiva)
            {
                NombreUsuario = SesionGlobal.UsuarioActual.Nombre + " " + SesionGlobal.UsuarioActual.Apellido1 + " " + SesionGlobal.UsuarioActual.Apellido2;
            }
            else
            {
                NombreUsuario = "Invitado";
            }
        }

        #endregion

        #region reloj
        private void IniciarReloj()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => HoraActual = DateTime.Now.ToString("HH:mm:ss | dd MMMM yyyy");
            _timer.Start();
        }
        #endregion

        #region KPIs

        //carga las tarjetas de los kpis rápidos
        private async Task CargarKpisAsync()
        {
            try
            {
                var proyectos = await _proyectoRepository.GetAllAsync();
                var trabajadores = await _trabajadorRepository.GetAllAsync();
                var maquinas = await _maquinaRepository.GetAllMaquinasAsync();
                var gastos = await _gastoRepository.GetAllAsync();
                var facturas = await _facturaRepository.GetAllAsync();


                int cantidadActivos = proyectos.Count(p => p.FechaFin == null || p.FechaFin >= DateTime.Now);

                int cantidadTotalTrabajadores = trabajadores.Count;
                int cantidadTrabajadoresActivos = trabajadores.Count(t => t.Estado == "Activo");

                int cantidadTotalMaquinas = maquinas.Count();
                int cantidadMaquinasOperativas = maquinas.Count(m => m.IdEstadoNavigation != null && m.IdEstadoNavigation.Descripcion == "En Uso");

                //int cantidadGastosMesActual = gastos.Count(g => g.Fecha.HasValue && g.Fecha.Value.Month == DateTime.Now.Month && g.Fecha.Value.Year == DateTime.Now.Year);
                //int cantidadFacturasMesActual = facturas.Count(f => f.Fecha.HasValue && f.Fecha.Value.Month == DateTime.Now.Month && f.Fecha.Value.Year == DateTime.Now.Year);
                int balanceMesActual = (int)(facturas.Where(f => f.Fecha.HasValue && f.Fecha.Value.Month == DateTime.Now.Month && f.Fecha.Value.Year == DateTime.Now.Year).Sum(f => f.Total ?? 0) -
                                    gastos.Where(g => g.Fecha.HasValue && g.Fecha.Value.Month == DateTime.Now.Month && g.Fecha.Value.Year == DateTime.Now.Year).Sum(g => g.Cantidad ?? 0));

                KpiProyectos = $"{cantidadActivos} Activos";
                KpiPlantillaActiva = $"{cantidadTrabajadoresActivos} / {cantidadTotalTrabajadores} Activos";
                KpiMaquinasOperativas = $"{cantidadMaquinasOperativas} / {cantidadTotalMaquinas} Operando";
                KpiBalance = $"{(balanceMesActual >= 0 ? "+ " : "- ")}{Math.Abs(balanceMesActual):N2} €";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proyectos: {ex.Message}");
                KpiProyectos = "Error";
            }
        }

        #endregion
    }
}
