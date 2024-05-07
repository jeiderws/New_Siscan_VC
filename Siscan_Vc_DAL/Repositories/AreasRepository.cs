using Microsoft.EntityFrameworkCore;
using Siscan_Vc_DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siscan_Vc_DAL.Repositories
{
    public class AreasRepository : IGenericRepository<AreasEmpresa>
    {
        private readonly DbSiscanContext _dbcontext;
        public AreasRepository(DbSiscanContext dbcontext)
        {

            _dbcontext = dbcontext;

        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                AreasEmpresa area = _dbcontext.AreasEmpresas.First(a => a.IdArea.ToString() == id);
                _dbcontext.AreasEmpresas.Remove(area);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        public async Task<IQueryable<AreasEmpresa>> GetAll()
        {
            try
            {
                IQueryable<AreasEmpresa> queryArea = _dbcontext.AreasEmpresas;
                return queryArea;
            }
            catch { return null; }
        }

        public async Task<AreasEmpresa> GetForId(string id)
        {
            try
            {
                return await _dbcontext.AreasEmpresas.FindAsync(id);
            }
            catch { return null; }
        }

        public async Task<bool> Insert(AreasEmpresa model)
        {
            try
            {
                _dbcontext.AreasEmpresas.Add(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch{ return false; }
        }

        public async Task<bool> Update(AreasEmpresa model)
        {
            try
            {
                _dbcontext.AreasEmpresas.Update(model);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }
    }
}
