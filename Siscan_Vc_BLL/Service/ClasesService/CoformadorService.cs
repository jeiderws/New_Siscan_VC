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
    public class CoformadorService : ICoformadorService
    {
        private readonly IGenericRepository<Coformador> _repository;

        public CoformadorService(IGenericRepository<Coformador> repoCoformador)
        {
            _repository = repoCoformador;
        }
        public Task<bool> Delete(string numDoc)
        {
            return _repository.Delete(numDoc);
        }

        public Task<IQueryable<Coformador>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<Coformador> GetForDoc(string numeroDoc)
        {
            try
            {
                IQueryable<Coformador> queryCoformador= await _repository.GetAll();
                Coformador coformador = queryCoformador.Where(c=>c.NumeroDocumentoCoformador==numeroDoc).FirstOrDefault();
                return coformador;
            }
            catch { return null; }
        }

        public async Task<Coformador> GetForEmpresa(string nitEmpresa)
        {
            try
            {
                IQueryable<Coformador> queryCoformador = await _repository.GetAll();
                Coformador coformador = queryCoformador.Where(c => c.NitEmpresa == nitEmpresa).FirstOrDefault();
                return coformador;
            }
            catch { return null; }
        }

        public async Task<Coformador> GetForId(string idCoformador)
        {
            return await _repository.GetForId(idCoformador);
        }

        public async Task<Coformador> GetForName(string name)
        {
            try
            {
                IQueryable<Coformador> queryCoformador = await _repository.GetAll();
                Coformador coformador = queryCoformador.Where(c => c.NombreCoformador == name).FirstOrDefault();
                return coformador;
            }
            catch { return null; }
        }

        public Task<bool> Insert(Coformador model)
        {
            return _repository.Insert(model);
        }

        public async Task<bool> Update(Coformador model)
        {
            return await _repository.Update(model);
        }
    }
}
