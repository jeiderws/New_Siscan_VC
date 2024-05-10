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
    public class InscripcionTYTService:IInscripcionTYTService
    {
        private readonly IGenericRepository<InscripcionTyt> _repositoryTYT;
        public InscripcionTYTService(IGenericRepository<InscripcionTyt> repositoryTYT)
        {
            _repositoryTYT = repositoryTYT;
        }

        public Task<bool> Delete(string cogInscripcion)
        {
            return _repositoryTYT.Delete(cogInscripcion);
        }

        public Task<IQueryable<InscripcionTyt>> GetAll()
        {
            return _repositoryTYT.GetAll();
        }

        public async Task<InscripcionTyt> GetForCogInscripcion(string cogInscripcion)
        {
            return await _repositoryTYT.GetForId(cogInscripcion);
        }

        public Task<bool> Insert(InscripcionTyt model)
        {
            return _repositoryTYT.Insert(model);
        }

        public async Task<bool> Update(InscripcionTyt model)
        {
            return await _repositoryTYT.Update(model);
        }
    }
}
