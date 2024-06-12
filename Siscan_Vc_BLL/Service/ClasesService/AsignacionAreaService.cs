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
    public class AsignacionAreaService : IAsignacionService
    {
        private readonly IGenericRepository<AsignacionArea> _areAsigRepository;
        public AsignacionAreaService(IGenericRepository<AsignacionArea> areAsigRepo)
        {
            _areAsigRepository = areAsigRepo;
        }
        public Task<bool> Delete(int id)
        {
            return _areAsigRepository.Delete(id.ToString());
        }

        public Task<IQueryable<AsignacionArea>> GetAll()
        {
            return _areAsigRepository.GetAll();
        }

        public async Task<AsignacionArea> GetForId(int id)
        {
            return await _areAsigRepository.GetForId(id.ToString());
        }

        public Task<bool> Insert(AsignacionArea model)
        {
            return _areAsigRepository.Insert(model);
        }

        public async Task<bool> Update(AsignacionArea model)
        {
            return await _areAsigRepository.Update(model);
        }
    }
}
