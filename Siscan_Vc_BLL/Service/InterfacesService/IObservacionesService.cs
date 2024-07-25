using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IObservacionesService
    {
        Task<bool> Insert(Observacion model);
        Task<bool> Update(Observacion model);
        Task<bool> Delete(string id);
        Task<Observacion> GetForId(string id);
        Task<IQueryable<Observacion>> GetAll();
        Task<Observacion> GetForSegui(int idsegui);
    }
}
