namespace jsolo.simpleinventory.sys.models;



public class InventoryTransactionViewModel
{
    public Guid Id { get; set; }

    public required InventoryViewModel Inventory { get; set; }

    public required InventoryTransactionTypeViewModel Type { get; set; }

    public DateTime TimeStamp { get; set; }

    public double Amount { get; set; }
}

public class InventoryTransactionTypeViewModel
{
    public char Value { get; set; }

    public required string Name { get; set; }
}


public class InventoryTransactionsFilterViewModel : QueryFilterViewModel
{
    public Guid? InventoryId { get; set; }

    public DateTime? Start { get; set; }

    public DateTime? End { get; set; }

    public double? Minimum { get; set; }

    public double? Maximum { get; set; }
}
