namespace jsolo.simpleinventory.sys.models;


public class InventoryViewModel
{
    public required Guid Id { get; set; }
    
    public required ProductViewModel Product { get; set; }

    public double StockCount { get; set; }

    public double MinimumStockCount { get; set; }

    public double MinimumReorderQuantity { get; set; }

    public List<InventoryTransactionViewModel> Transactions { get; set; } = [];
}



public class InventoriesFilterViewModel : QueryFilterViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public string? ProductName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? InternalProductNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExternalProductNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ProductType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ProductMake { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ProductBarcode { get; set; }
}
