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
    public class SeguimientoService : ISeguimientoService
    {
        private readonly IGenericRepository<SeguimientoInstructorAprendiz> _seguimientorepo;
        public SeguimientoService(IGenericRepository<SeguimientoInstructorAprendiz> seguimientorepo)
        {
            _seguimientorepo = seguimientorepo;
        }

        public Task<bool> Delete(long id)
        {
            return _seguimientorepo.Delete(id.ToString());
        }

        public Task<IQueryable<SeguimientoInstructorAprendiz>> GetAll()
        {
            return _seguimientorepo.GetAll();
        }

        public async Task<SeguimientoInstructorAprendiz> GetForId(long id)
        {
            return await _seguimientorepo.GetForId(id.ToString());
        }

        public async Task<SeguimientoInstructorAprendiz> GetForNumDocAprdz(string numDoc)
        {
            try
            {
                IQueryable<SeguimientoInstructorAprendiz> querySegui = await _seguimientorepo.GetAll();
                SeguimientoInstructorAprendiz segui = querySegui.Where(a => a.NumeroDocumentoAprendiz == numDoc).FirstOrDefault();
                return segui;
            }
            catch { return null; }
        }

        public Task<bool> Insert(SeguimientoInstructorAprendiz model)
        {
            return _seguimientorepo.Insert(model);
        }

        public async Task<bool> Update(SeguimientoInstructorAprendiz model)
        {
            return await _seguimientorepo.Update(model);
        }

      
    }
}
