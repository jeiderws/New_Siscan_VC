using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface IAsignacionService
    {
        Task<bool> Insert(AsignacionArea model);
        Task<bool> Update(AsignacionArea model);
        Task<bool> Delete(int id);
        Task<AsignacionArea> GetForId(int id);
        Task<IQueryable<AsignacionArea>> GetAll();
    }
}
