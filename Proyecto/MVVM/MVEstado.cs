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
    public class MVEstado : MVBase
    {
        private EstadoRepository _estadoRepository;
        private Estado _estado;

        public Estado estado
        {
            get => _estado;
            set => SetProperty(ref _estado, value);
        }

        public MVEstado(EstadoRepository estadoRepository) 
        {
            _estadoRepository = estadoRepository;
            estado = new Estado();
        }

        public async Task GuardarEstado()
        {
            var estadoExistente = await _estadoRepository.FirstOrDefaultAsync(x => x.Descripcion == estado.Descripcion);
            try
            {
                if (estadoExistente == null)
                {
                    await _estadoRepository.AddAsync(estado);
                }
                else
                {
                    await _estadoRepository.UpdateAsync(estado);
                }
                SnackbarMessageQueue.Enqueue("Estado guardado correctamente.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el estado: " + ex.Message);
            }
        }
    }
}
