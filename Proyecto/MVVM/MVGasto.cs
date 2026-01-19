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
    public class MVGasto : MVBase
    {
        private Gasto _gasto;
        private GastoRepository _gastoRepository;

        private List<Gasto> _gastoList;


        public List<Gasto> gastoList => _gastoList;

        public MVGasto(GastoRepository gastoRepository)
        {
            _gastoRepository = gastoRepository;
            _gasto = new Gasto();
        }

        public async Task Inicializa()
        {
            _gastoList = await GetAllAsync<Gasto>(_gastoRepository);
        }

    }
}
