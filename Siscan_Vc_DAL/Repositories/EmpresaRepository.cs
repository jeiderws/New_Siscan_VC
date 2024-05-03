using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class EmpresaRepository : IGenericRepository<Empresa>
    {
        private readonly DbSiscanContext _dbcontext;
        public EmpresaRepository(DbSiscanContext dbcontext)
        {

            _dbcontext = dbcontext;

        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                Empresa empresa=_dbcontext.Empresas.First(e=>e.Nitmpresa==id);
                _dbcontext.Remove(empresa);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<Empresa>> GetAll()
        {
            try
            {
                IQueryable<Empresa> queryEmpresa = _dbcontext.Empresas.Include(c => c.IdCiudadNavigation);
                return queryEmpresa;
            }
            catch { return null; }
        }

        public async Task<Empresa> GetForId(string id)
        {
            try
            {
                return await _dbcontext.Empresas.FindAsync(id)
            }
            catch { return null; }
        }

        public async Task<bool> Insert(Empresa model)
        {
            try
            {
                _dbcontext.Empresas.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> Update(Empresa model)
        {
            try
            {
                _dbcontext.Empresas.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
