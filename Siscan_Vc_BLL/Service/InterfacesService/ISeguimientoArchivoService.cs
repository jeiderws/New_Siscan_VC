using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_BLL.Service.InterfacesService
{
    public interface ISeguimientoArchivoService
    {
        Task<bool> Insert(SeguimientoArchivo model);
        Task<IQueryable<SeguimientoArchivo>> GetAll();
        Task<IQueryable<SeguimientoArchivo>> GetForDocAprendiz(string nmDocAprendiz);
    }
}
