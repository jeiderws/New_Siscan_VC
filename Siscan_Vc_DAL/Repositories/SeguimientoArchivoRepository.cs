using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class SeguimientoArchivoRepository : IGenericRepository<SeguimientoArchivo>
    {
        private readonly DbSiscanContext _dbContext;
        public SeguimientoArchivoRepository(DbSiscanContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryable<SeguimientoArchivo>> GetAll()
        {
            try
            {
                IQueryable<SeguimientoArchivo> querySegui = _dbContext.SeguimientoArchivos;
                return querySegui;
            }
            catch { return null; }
        }

        public Task<SeguimientoArchivo> GetForId(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(SeguimientoArchivo model)
        {
            try
            {
                _dbContext.SeguimientoArchivos.Add(model);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public Task<bool> Update(SeguimientoArchivo model)
        {
            throw new NotImplementedException();
        }
    }
}
