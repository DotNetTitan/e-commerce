using Ecommerce.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Saves all changes made in this context to the database and commits the transaction.
        /// This method should be called after BeginTransactionAsync() for operations requiring transaction management.
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        /// <summary>
        /// Discards all changes made in this context and rolls back the transaction.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Saves all changes made in this context to the database without transaction management.
        /// Use this method for simple operations that don't require explicit transaction control.
        /// </summary>
        public async Task<int> SaveChangesWithoutTransactionAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
        }
    }
}