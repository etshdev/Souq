using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Souq.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(SouqDB SouqDB) => this.SouqDB = SouqDB;

        public SouqDB SouqDB { get; }


        public IDbContextTransaction dbContextTransaction { get; set; }

        public void commit()
        {
            SouqDB.Database.CurrentTransaction.Commit();
        }

        public void Dispose()
        {
            SouqDB.Dispose();
        }

        public bool SaveChanges()
        {
            try
            {
                return SouqDB.SaveChanges() > 0 ? true : false;
            }
            catch(Exception)
            {

                return false;
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return (await SouqDB.SaveChangesAsync()) > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
    }
}
