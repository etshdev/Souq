using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Souq.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {

        SouqDB SouqDB { get; }

        IDbContextTransaction dbContextTransaction { get; set; }

        void commit();

        bool SaveChanges();

        Task<bool> SaveChangesAsync();

    }
}
