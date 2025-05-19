using jsolo.simpleinventory.core.common;
using jsolo.simpleinventory.core.objects;



namespace jsolo.simpleinventory.core.entities;


///<summary>An record of the current stock levels of a specified <see cref="Product">.</summary>
public class Inventory : Entity<Guid>
{
    #region properties

    /// <summary>. . .</summary>
    public virtual Product Item { get; protected set; }

    // /// <summary>. . .</summary>
    // public virtual Location Location { get; protected set; }

    /// <summary>. . .</summary>
    public virtual double StockCount { get; protected set; }

    /// <summary>. . .</summary>
    public virtual double MinimumQuantity { get; protected set; }

    /// <summary>. . .</summary>
    public virtual double ReOrderQuantity { get; protected set; }

    /// <summary>. . .</summary>
    public virtual IList<InventoryTransaction> TransactionsHistory { get; protected set; } = new List<InventoryTransaction>();

    #endregion


    #region constructors
    /// <summary>
    /// Creates a new <see cref="Inventory"/>. Reserved for use by ORM.
    /// </summary>
    protected Inventory() { }


    /// <summary>
    /// Creates a new <see cref="Inventory"/>.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="item"></param>
    /// <param name="location"></param>
    /// <param name="stockCount"></param>
    /// <param name="minimumQty"></param>
    /// <param name="reorderQty"></param>
    /// <param name="createdOn"></param>
    /// <param name="creatorId"></param>
    /// <param name="transactions"></param>
    /// <param name="lastUpdatedOn"></param>
    /// <param name="lastUpdaterId"></param>
    /// <returns>An <see cref="Inventory"/> instance.</returns>
    public Inventory(
        Guid id,
        Product item,
        double stockCount,
        double minimumQty,
        double reorderQty,
        DateTime createdOn,
        string creatorId,
#nullable enable
        // Location? location = null,
        IEnumerable<InventoryTransaction>? transactions = null,
        DateTime? lastUpdatedOn = null,
        string? lastUpdaterId = null
#nullable disable
    )
    {
        if (id.Equals(Guid.Empty))
        {
            throw new ArgumentException("Invalid inventory ID supplied.", nameof(id));
        }

        this.Id = id;

        this.CreatedOn = createdOn;
        this.CreatorId = creatorId;

        this.SetProduct(item)
            // .SetLocationAs(location)
            .SetStockCountAs(stockCount)
            .SetMinimumQuantityAs(minimumQty)
            .SetReorderQuantityAs(reorderQty)
            .SetTransactionsAs(transactions)
            .SetLastModifierAsAt(lastUpdaterId, lastUpdatedOn);
    }
    #endregion


    #region instance methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public Inventory SetProduct(Product product)
    {
        this.Item = product;

        return this;
    }


    // /// <summary>
    // /// 
    // /// </summary>
    // /// <param name="location"></param>
    // /// <returns></returns>
    // public Inventory SetLocationAs(Location location)
    // {
    //     this.Location = location;

    //     return this;
    // }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Inventory SetStockCountAs(double quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Stock count cannot be less than zero (0).");
        }

        this.StockCount = quantity;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Inventory SetMinimumQuantityAs(double quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Minimum required stock count cannot be less than zero (0).");
        }

        this.MinimumQuantity = quantity;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Inventory SetReorderQuantityAs(double quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Re-order quantity must be more than zero (0).");
        }

        this.MinimumQuantity = quantity;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns></returns>
    public Inventory SetTransactionsAs(
#nullable enable
        IEnumerable<InventoryTransaction>? transactions = null
#nullable disable
    )
    {
        this.TransactionsHistory.Clear();

        if (transactions?.Count() > 0)
        {
            foreach (InventoryTransaction transaction in transactions)
            {
                this.TransactionsHistory.Add(transaction);
            }
        }

        return this;
    }
    #endregion


    #region implementations and overrides
    protected override IEnumerable<object> GetEqualityComponents() =>
    [
        this.Item.Id,
        // this.Location?.FullLocation ?? ""
    ];
    #endregion
}


public static class InventoryItemEntensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static bool IsStockLow(this Inventory item)
    {
        if (item.MinimumQuantity > 0)
        {
            return item.StockCount <= item.MinimumQuantity;
        }
        return false;
    }
}
