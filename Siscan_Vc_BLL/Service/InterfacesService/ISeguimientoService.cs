using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface ISeguimientoService
    {
        Task<bool> Insert(SeguimientoInstructorAprendiz model);
        Task<bool> Update(SeguimientoInstructorAprendiz model);
        Task<bool> Delete(int id);
        Task<SeguimientoInstructorAprendiz> GetForId(int id);
        Task<IQueryable<SeguimientoInstructorAprendiz>> GetAll();
        Task<SeguimientoInstructorAprendiz> GetForNumDocAprdz(string numDoc);
    }
}
