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
    public class ActividadService : IActividadService
    {
        private readonly IGenericRepository<Actividade> _repository;
        public ActividadService(IGenericRepository<Actividade> repositoryActividad)
        {
                _repository = repositoryActividad;
        }
        public Task<bool> Delete(string id)
        {
            return _repository.Delete(id);
        }

        public Task<IQueryable<Actividade>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<Actividade> GetForId(string id)
        {
            return await _repository.GetForId(id);
        }

        public async Task<Actividade> GetForSegui(int idsegui)
        {
            try
            {
                IQueryable<Actividade> queryactivi = await _repository.GetAll();
                Actividade actividad = queryactivi.Where(a => a.IdSeguimiento == idsegui).FirstOrDefault();
                return actividad;
            }
            catch {return null;}
        }

        public Task<bool> Insert(Actividade model)
        {
            return _repository.Insert(model);
        }

        public async Task<bool> Update(Actividade model)
        {
           return await _repository.Update(model);
        }
    }
}
