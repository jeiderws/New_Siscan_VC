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
    public class AreasService:IAreasService
    {
        private readonly IGenericRepository<AreasEmpresa> _repositoryAreas;
        public AreasService(IGenericRepository<AreasEmpresa> repoAreas)
        {
            _repositoryAreas = repoAreas;
        }
        public Task<bool> Delete(int id)
        {
            return _repositoryAreas.Delete(id.ToString());
        }

        public Task<IQueryable<AreasEmpresa>> GetAll()
        {
            return _repositoryAreas.GetAll();
        }

        public async Task<AreasEmpresa> GetForId(int id)
        {
            return await _repositoryAreas.GetForId(id.ToString());
        }

        public async Task<AreasEmpresa> GetForName(string name)
        {
            try
            {
                IQueryable<AreasEmpresa> queryAreas=await _repositoryAreas.GetAll();
                AreasEmpresa area=queryAreas.Where(a=>a.NombreArea==name).FirstOrDefault();
                return area;
            }
            catch { return null; }
        }

        public Task<bool> Insert(AreasEmpresa model)
        {
            return _repositoryAreas.Insert(model);
        }

        public async Task<bool> Update(AreasEmpresa model)
        {
            return await _repositoryAreas.Update(model);
        }
    }
}
