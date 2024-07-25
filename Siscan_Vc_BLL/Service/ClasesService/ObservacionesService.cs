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
    public class ObservacionesService : IObservacionesService
    {
        private readonly IGenericRepository<Observacion> _repository;
        public ObservacionesService(IGenericRepository <Observacion> repositoryObservacion)
        {
                _repository = repositoryObservacion;
        }
        public Task<bool> Delete(string id)
        {
            return _repository.Delete(id);
        }

        public Task<IQueryable<Observacion>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<Observacion> GetForId(string id)
        {
            return await _repository.GetForId(id);
        }

        public async Task<Observacion> GetForSegui(int idsegui)
        {
            try
            {
                IQueryable<Observacion> queryobserv = await _repository.GetAll();
                Observacion observacion = queryobserv.Where(o=> o.IdSeguimiento == idsegui).FirstOrDefault();
                return observacion;
            }
            catch {return null;}
        }

        public Task<bool> Insert(Observacion model)
        {
            return _repository.Insert(model);
        }

        public async Task<bool> Update(Observacion model)
        {
            return await _repository.Update(model);
        }
    }
}
