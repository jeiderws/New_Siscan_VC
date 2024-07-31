using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class ActividadRepository : IGenericRepository<Actividade>
    {
        private readonly DbSiscanContext _dbcontext;
        public ActividadRepository(DbSiscanContext dbcontext)
        {
                _dbcontext = dbcontext;
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                Actividade activi = _dbcontext.Actividades.First(a => a.IdActividad.ToString() == id);
                _dbcontext.Remove(activi);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false;   }
        }

        public async Task<IQueryable<Actividade>> GetAll()
        {
            try
            {
                IQueryable<Actividade> queryacti = _dbcontext.Actividades.Include(a => a.IdSeguimientoNavigation);
                return queryacti;
            }
            catch { return null;  }
        }

        public async Task<Actividade> GetForId(string id)
        {
            try
            {
                return await _dbcontext.Actividades.FindAsync(id);
            }
            catch {return null;}
        }

        public async Task<bool> Insert(Actividade model)
        {
            try
            {
                _dbcontext.Actividades.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch {return false;}
        }

        public async Task<bool> Update(Actividade model)
        {
            try
            {
                _dbcontext.Actividades.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch {return false;}
        }
    }
}
