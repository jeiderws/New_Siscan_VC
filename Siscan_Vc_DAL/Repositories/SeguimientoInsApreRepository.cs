using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class SeguimientoInsApreRepository : IGenericRepository<SeguimientoInstructorAprendiz>
    {
        private readonly DbSiscanContext _dbSiscanContext;
        public SeguimientoInsApreRepository(DbSiscanContext dbSiscanContext)
        {
            _dbSiscanContext = dbSiscanContext;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                SeguimientoInstructorAprendiz segui = _dbSiscanContext.SeguimientoInstructorAprendizs.First(a => a.IdSeguimiento.ToString() == id);
                _dbSiscanContext.Remove(segui);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<SeguimientoInstructorAprendiz>> GetAll()
        {
            try
            {
                IQueryable<SeguimientoInstructorAprendiz> querySegui = _dbSiscanContext.SeguimientoInstructorAprendizs.Include(a => a.NumeroDocumentoAprendizNavigation).Include(x => x.NumeroDocumentoInstructorNavigation).Include(z => z.IdAreaEmpresaNavigation)
                    .Include(z => z.IdAsignacionAreaNavigation).Include(z => z.IdCoformadorNavigation).Include(z => z.IdModalidadNavigation);
                return querySegui;
            }
            catch { return null; }
        }

        public async Task<SeguimientoInstructorAprendiz> GetForId(string id)
        {
            try
            {
                return await _dbSiscanContext.SeguimientoInstructorAprendizs.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(SeguimientoInstructorAprendiz model)
        {
            try
            {
                _dbSiscanContext.SeguimientoInstructorAprendizs.Add(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(SeguimientoInstructorAprendiz model)
        {
            try
            {
                _dbSiscanContext.SeguimientoInstructorAprendizs.Update(model);
                await _dbSiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
