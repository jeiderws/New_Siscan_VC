using Siscan_Vc_BLL.Service.InterfacesService;
using Siscan_Vc_DAL.DataContext;
using Siscan_Vc_DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.ClasesService
{
    public class AsignacionFichasService : IAsigancionFichas
    {
        private readonly IGenericRepository<AsignacionFicha> _AsigFichaRepository;
        public AsignacionFichasService(IGenericRepository<AsignacionFicha> AsigFichaRepository)
        {
                _AsigFichaRepository = AsigFichaRepository;
        }
        public Task<bool> Delete(int id)
        {
           return _AsigFichaRepository.Delete(id.ToString());
        }

        public Task<IQueryable<AsignacionFicha>> GetAll()
        {
            return _AsigFichaRepository.GetAll();
        }

        public async Task<AsignacionFicha> GetForId(int id)
        {
            return await _AsigFichaRepository.GetForId(id.ToString());
        }

        public Task<bool> Insert(AsignacionFicha model)
        {
            return _AsigFichaRepository.Insert(model);
        }

        public async Task<bool> Update(AsignacionFicha model)
        {
            return await _AsigFichaRepository.Update(model);
        }
    }
}
