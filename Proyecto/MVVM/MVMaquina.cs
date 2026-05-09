using Proyecto.Backend.Modelo;
using Proyecto.Backend.Repositorios;
using Proyecto.MVVM.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Proyecto.MVVM
{
    public class MVMaquina : MVBase
    {
        #region miembros privados
        private Maquina _maquina;
        private string _nombreModelo;
        private string _nombreEstado;

        private List<Modelo> _listaModelos;
        private List<Estado> _listaEstados;
        private ObservableCollection<Maquina> _listaMaquinas;
        private Maquina _maquinaSeleccionada;

        private MaquinaRepository _maquinaRepository;
        private ModeloRepository _modeloRepository;
        private EstadoRepository _estadoRepository;
        #endregion

        #region miembros públicos
        public List<Modelo> listaModelos => _listaModelos;
        public List<Estado> listaEstados => _listaEstados;
        public Maquina maquina
        {
            get => _maquina;
            set => SetProperty(ref _maquina, value);
        }

        public string NombreModelo
        {
            get => _nombreModelo;
            set => SetProperty(ref _nombreModelo, value);
        }

        public string NombreEstado
        {
            get => _nombreEstado;
            set => SetProperty(ref _nombreEstado, value);
        }

        public ObservableCollection<Maquina> ListaMaquinas
        {
            get => _listaMaquinas;
            set => SetProperty(ref _listaMaquinas, value);
        }

        public Maquina MaquinaSeleccionada
        {
            get => _maquinaSeleccionada;
            set => SetProperty(ref _maquinaSeleccionada, value);
        }
        #endregion

        #region constructor
        public MVMaquina(MaquinaRepository maquinaRepository, ModeloRepository modeloRepository, EstadoRepository estadoRepository)
        {
            _maquinaRepository = maquinaRepository;
            _modeloRepository = modeloRepository;
            _estadoRepository = estadoRepository;
            maquina = new Maquina();
        }
        #endregion

        // Función inicializa
        public async Task Inicializa()
        {
            _listaModelos = await GetAllAsync<Modelo>(_modeloRepository);
            _listaEstados = await GetAllAsync<Estado>(_estadoRepository);
            var maquinas = await GetAllAsync<Maquina>(_maquinaRepository);
            ListaMaquinas = new ObservableCollection<Maquina>(maquinas);
        }

        // Función guardar
        public async Task GuardarMaquina()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreModelo))
                {
                    throw new Exception("El campo Modelo es obligatorio.");
                }

                var modeloExistente = await _modeloRepository.FirstOrDefaultAsync(m => m.ModeloMaquina == NombreModelo, false);

                if (modeloExistente == null)
                {
                    modeloExistente = new Modelo()
                    {
                        ModeloMaquina = NombreModelo
                    };

                    await _modeloRepository.AddAsync(modeloExistente);
                }

                maquina.IdModeloNavigation = modeloExistente;

                var estadoExistente = await _estadoRepository.FirstOrDefaultAsync(e => e.Descripcion == NombreEstado, false);
                if (estadoExistente == null)
                {
                    estadoExistente = new Estado()
                    {
                        Descripcion = NombreEstado 
                    };
                    await _estadoRepository.AddAsync(estadoExistente);
                }
                maquina.IdEstadoNavigation = estadoExistente;

                if (maquina.IdMaquina == 0)
                {
                    await _maquinaRepository.AddAsync(maquina);
                }
                else
                {
                    await _maquinaRepository.UpdateAsync(maquina);
                }
                SnackbarMessageQueue.Enqueue("Maquina guardada correctamente.");
            }
            catch (Exception ex)
            {
                // Extraemos el error principal
                string errorReal = ex.Message;

                // Si Entity Framework tiene un detalle más profundo (que casi siempre lo tiene), lo sacamos:
                if (ex.InnerException != null)
                {
                    errorReal += "\n\nDETALLE DE LA BASE DE DATOS:\n" + ex.InnerException.Message;
                }

                // Lo mostramos en pantalla para que lo leas fácilmente
                System.Windows.MessageBox.Show(errorReal, "Error al guardar");

                throw new Exception("Error al guardar la maquina: " + errorReal);
            }
        }

        // Función para dar de baja (Borrado lógico)
        public async Task DarDeBajaMaquina(Maquina maquinaABorrar)
        {
            if (maquinaABorrar == null) return;

            try
            {
                // 1. Buscamos el estado "Baja" (o "Inactiva") en la tabla de Estados
                var estadoBaja = await _estadoRepository.FirstOrDefaultAsync(e => e.Descripcion == "Baja", false);

                // 2. Si el estado "Baja" no existe en la base de datos, lo creamos
                if (estadoBaja == null)
                {
                    estadoBaja = new Estado()
                    {
                        Descripcion = "Baja"
                    };
                    await _estadoRepository.AddAsync(estadoBaja);
                }

                // 3. Le asignamos este estado a la máquina seleccionada
                maquinaABorrar.IdEstadoNavigation = estadoBaja;
                maquinaABorrar.IdEstado = estadoBaja.IdEstado;

                // 4. Actualizamos la máquina en la base de datos
                await _maquinaRepository.UpdateAsync(maquinaABorrar);

                // 5. Refrescamos la lista para que la vista se actualice
                await Inicializa();

                SnackbarMessageQueue.Enqueue("Máquina dada de baja correctamente.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error al dar de baja la máquina: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
