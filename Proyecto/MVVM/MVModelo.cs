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
    public class MVModelo : MVBase
    {

        #region miembros privados
        private ModeloRepository _modeloRepository;
        private Modelo _modelo;


        #endregion

        #region miembros públicos
        public Modelo modelo
        {
            get => _modelo;
            set => SetProperty(ref _modelo, value);
        }

        #endregion

        public MVModelo(
            ModeloRepository modeloRepository
        ) 
        {
            _modeloRepository = modeloRepository;
            modelo = new Modelo();
        
        }

        public async Task GuardarModelo()
        {
            var proyectoExistente = await _modeloRepository.FirstOrDefaultAsync(x => x.ModeloMaquina == modelo.ModeloMaquina);
            try
            {
                if (proyectoExistente == null)
                {
                    await _modeloRepository.AddAsync(modelo);
                }
                else
                {
                    await _modeloRepository.UpdateAsync(modelo);
                }
                SnackbarMessageQueue.Enqueue("Modelo guardado correctamente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el modelo: " + ex.Message);
            }
        }
    }
}
