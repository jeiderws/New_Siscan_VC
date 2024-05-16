using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class ProgramasRepository : IGenericRepository<Programas>
    {
        private readonly DbSiscanContext _dbsiscanContext;
        public ProgramasRepository(DbSiscanContext dbSiscanContext)
        {
            _dbsiscanContext = dbSiscanContext;
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                Programas programas = _dbsiscanContext.Programas.First(p => p.CodigoPrograma == id);
                _dbsiscanContext.Remove(programas);
                await _dbsiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Programas>> GetAll()
        {
            try
            {
                IQueryable<Programas> queryPrograma = _dbsiscanContext.Programas.Include(n => n.IdNivelProgramaNavigation).Include(e => e.IdEstadoProgramaNavigation);
                return queryPrograma;
            }
            catch { return null; }
        }

        public async Task<Programas> GetForId(string id)
        {
            try
            {
                return await _dbsiscanContext.Programas.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(Programas model)
        {
            try
            {
                _dbsiscanContext.Programas.Add(model);
                await _dbsiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Programas model)
        {
            try
            {
                _dbsiscanContext.Programas.Update(model);
                await _dbsiscanContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
