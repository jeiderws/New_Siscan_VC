using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class AsignacionAreaRepository : IGenericRepository<AsignacionArea>
    {
        private readonly DbSiscanContext _contextdb;
        public AsignacionAreaRepository(DbSiscanContext contextdb)
        {

            _contextdb = contextdb;

        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                AsignacionArea asigArea=_contextdb.AsignacionAreas.First(a=>a.IdAsignacionArea.ToString()==id);
                _contextdb.Remove(asigArea);
                await _contextdb.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<AsignacionArea>> GetAll()
        {
            try
            {
                IQueryable<AsignacionArea> queryAsigArea = _contextdb.AsignacionAreas.Include(a => a.IdAreaNavigation).Include(e => e.NitEmpresaNavigation);
                return queryAsigArea;
            }
            catch { return null; }
        }

        public async Task<AsignacionArea> GetForId(string id)
        {
            try
            {
                return await _contextdb.AsignacionAreas.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(AsignacionArea model)
        {
            try
            {
                _contextdb.AsignacionAreas.Add(model);
                await _contextdb.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(AsignacionArea model)
        {
            try
            {
                _contextdb.AsignacionAreas.Update(model);
                await _contextdb.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
