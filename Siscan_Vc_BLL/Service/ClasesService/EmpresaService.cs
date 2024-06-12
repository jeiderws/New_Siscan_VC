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
    public class EmpresaService : IEmpresaService
    {
        private readonly IGenericRepository<Empresa> _repositoryempresa;
        public EmpresaService(IGenericRepository<Empresa> repoempresa)
        {
            _repositoryempresa = repoempresa;
        }
        public Task<bool> Delete(string nit)
        {
              return _repositoryempresa.Delete(nit);
        }

        public Task<IQueryable<Empresa>> GetAll()
        {
            return _repositoryempresa.GetAll();
        }

        public async Task<Empresa> GetForNit(string nit)
        {
            return await _repositoryempresa.GetForId(nit);
        }

        public async Task<Empresa> GetForName(string name)
        {
            try
            {
                IQueryable<Empresa> queryEmpresa= await _repositoryempresa.GetAll();
                Empresa empresa= queryEmpresa.Where(e=>e.NombreEmpresa==name).FirstOrDefault();
                return empresa;
            }
            catch { return null; }
        }

        public Task<bool> Insert(Empresa model)
        {
           return _repositoryempresa.Insert(model);
        }

        public async Task<bool> Update(Empresa model)
        {
            return await _repositoryempresa.Update(model);
        }
    }
}
