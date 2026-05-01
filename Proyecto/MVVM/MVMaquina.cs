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
    public class MVMaquina : MVBase
    {
        #region miembros privados
        private Maquina _maquina;
        private string _nombreModelo;
        private string _nombreEstado;

        private List<Modelo> _listaModelos;
        private List<Estado> _listaEstados;

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

    }
}
