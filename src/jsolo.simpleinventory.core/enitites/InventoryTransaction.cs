using jsolo.simpleinventory.core.common;
using jsolo.simpleinventory.core.enums;



namespace jsolo.simpleinventory.core.entities;


public class InventoryTransaction : Entity<Guid>
{
    #region properties
    /// <summary>
    /// 
    /// </summary>
    public DateTime Date { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public IntenventoryTransactionType Type { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public Inventory Inventory { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public double Amount { get; protected set; }
    #endregion


    #region constructors
    /// <summary>
    /// Creates a new <see cref="InventoryTransaction">. Reserved for use by ORM.
    /// </summary>
    protected InventoryTransaction() { }


    /// <summary>
    /// Creates a new <see cref="InventoryTransaction">.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="date"></param>
    /// <param name="transactionType"></param>
    /// <param name="inventory"></param>
    /// <param name="amount"></param>
    /// <param name="createdOn"></param>
    /// <param name="creatorId"></param>
    /// <param name="lastUpdatedOn"></param>
    /// <param name="lastUpdaterId"></param>
    public InventoryTransaction(
        Guid id,
        DateTime date,
        IntenventoryTransactionType transactionType,
        Inventory inventory,
        double amount,
        DateTime createdOn,
        string creatorId,
#nullable enable
        DateTime? lastUpdatedOn = null,
        string? lastUpdaterId = null
#nullable disable
    )
    {
        this.Id = id;

        this.CreatedOn = createdOn;
        this.CreatorId = creatorId;

        this.SetTransactionDate(date)
            .SetTransactionType(transactionType)
            .SetLinkedInventoryAs(inventory)
            .SetAmount(amount)
            .SetLastModifierAsAt(lastUpdaterId, lastUpdatedOn);
    }
    #endregion

    #region instance methods & properties
    /// <summary>
    /// 
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public InventoryTransaction SetTransactionDate(DateTime date)
    {
        this.Date = date;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="transactionType"></param>
    /// <returns></returns>
    public InventoryTransaction SetTransactionType(IntenventoryTransactionType transactionType)
    {
        this.Type = transactionType;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inventory"></param>
    /// <returns></returns>
    public InventoryTransaction SetLinkedInventoryAs(Inventory inventory)
    {
        this.Inventory = inventory;

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public InventoryTransaction SetAmount(double amount)
    {
        this.Amount = amount;

        return this;
    }
    #endregion


    #region  implementatiions and overrides
    protected sealed override IEnumerable<object> GetEqualityComponents() =>
    [
        this.Date,
        this.Inventory,
        this.Type,
        this.Amount,
    ];
    #endregion
}
