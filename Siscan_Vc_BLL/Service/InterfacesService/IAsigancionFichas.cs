using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IAsigancionFichas
    {
        Task<bool> Insert(AsignacionFicha model);
        Task<bool> Update(AsignacionFicha model);
        Task<bool> Delete(int id);
        Task<AsignacionFicha> GetForId(int id);
        Task<IQueryable<AsignacionFicha>> GetAll();
    }
}
