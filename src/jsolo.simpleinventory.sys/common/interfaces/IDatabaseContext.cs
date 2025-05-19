using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using jsolo.simpleinventory.core.entities;
using jsolo.simpleinventory.core.objects;


namespace jsolo.simpleinventory.sys.common.interfaces
{
    public interface IDatabaseContext : IDisposable
    {
        #region  data sets
        /// <summary>
        /// 
        /// </summary>
        IQueryable<InventoryTransaction> InventoryTransactions { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<Inventory> Inventories { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<Product> Products { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<Vendor> Vendors { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<ProductType> ProductTypes { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<Currency> Currencies { get; }
        #endregion

        
        #region CRUD functions
        IDatabaseContext Add<Entity>(Entity entity);


        Task<IDatabaseContext> AddAsync<Entity>(Entity entity, CancellationToken cancellationToken);


        IDatabaseContext Update<Entity>(Entity entity);


        Task<IDatabaseContext> UpdateAsync<Entity> (Entity entity, CancellationToken cancellationToken);


        IDatabaseContext Delete<Entity>(Entity entity);


        Task<IDatabaseContext> DeleteAsync<Entity> (Entity entity, CancellationToken cancellationToken);


        IDatabaseContext BeginTransaction();


        IDatabaseContext SaveChanges();


        Task<IDatabaseContext> SaveChangesAsync();
        

        IDatabaseContext RollbackChanges();
        

        Task<IDatabaseContext> RollbackChangesAsync();
        

        IDatabaseContext CloseTransaction();
        #endregion
    }
}
