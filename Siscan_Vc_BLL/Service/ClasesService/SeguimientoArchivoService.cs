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
    public class SeguimientoArchivoService : ISeguimientoArchivoService
    {
        private readonly IGenericRepository<SeguimientoArchivo> _repository;
        public SeguimientoArchivoService(IGenericRepository<SeguimientoArchivo> repository)
        {
            _repository = repository;
        }
        public Task<IQueryable<SeguimientoArchivo>> GetAll()
        {
            return _repository.GetAll();
        }

        public async Task<IQueryable<SeguimientoArchivo>> GetForDocAprendiz(string nmDocAprendiz)
        {
            try
            {
                IQueryable<SeguimientoArchivo> querySeguimiento= await _repository.GetAll();
                IQueryable<SeguimientoArchivo> seguimientoArchivos= querySeguimiento.Where(s=>s.NumeroDocumentoAprendiz==nmDocAprendiz);
                return seguimientoArchivos;
            }
            catch
            { return null; }
        }

        public Task<bool> Insert(SeguimientoArchivo model)
        {
            return _repository.Insert(model);
        }
    }
}
