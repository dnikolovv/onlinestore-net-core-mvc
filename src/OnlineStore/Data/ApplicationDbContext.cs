namespace OnlineStore.Data
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Models;

    public class ApplicationDbContext : IdentityDbContext<User, UserRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        private IDbContextTransaction currentTransaction;

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public virtual void BeginTransaction()
        {
            if (this.currentTransaction != null)
            {
                return;
            }

            this.currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public virtual async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                this.currentTransaction?.Commit();
            }
            catch (Exception)
            {
                RollbackTransaction();
            }
            finally
            {
                if (this.currentTransaction != null)
                {
                    this.currentTransaction.Dispose();
                    this.currentTransaction = null;
                }
            }
        }

        public virtual void RollbackTransaction()
        {
            try
            {
                this.currentTransaction?.Rollback();
            }
            finally
            {
                if (this.currentTransaction != null)
                {
                    this.currentTransaction.Dispose();
                    this.currentTransaction = null;
                }
            }
        }
    }
}
