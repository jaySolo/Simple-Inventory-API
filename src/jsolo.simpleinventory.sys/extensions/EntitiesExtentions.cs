using System.Collections.Generic;
using System.Linq;

using jsolo.simpleinventory.core.entities;
using jsolo.simpleinventory.core.objects;
using jsolo.simpleinventory.sys.models;



namespace jsolo.simpleinventory.sys.extensions;



public static class EntitiesExtentions
{
    public static CurrencyViewModel ToViewModel(this Currency currency) => new()
    {
        Code = currency.Code,
        Name = currency.Name,
        Symbol = currency.Symbol,
        Description = currency.Description
    };

    public static MoneyViewModel ToViewModel(this Money money) => new()
    {
        Amount = money.Amount,
        Currency = money.Currency.ToViewModel()
    };


    public static ProductTypeViewModel ToViewModel(this ProductType productType) => new()
    {
        Name = productType.Name,
        Description = productType.Description
    };


    public static ProductViewModel ToViewModel(this Product product) => new()
    {
        Id = product.Id,
        InternalProductNumber = product.InternalProductNumber,
        ExternalProductNumber = product.ExternalProductNumber,
        Name = product.ProductName,
        Type = product.Type.ToString(),
        Make = product.Make,
        Description = product.Description,
        MarketValue = product.MarketPrice.ToViewModel(),
        Barcode = product.BarCode,
        // Gallery = ,
        Suppliers = product?.Suppliers?.Count > 0 ? product.Suppliers.Select(supplier => supplier.ToViewModel()).ToList() : new List<VendorViewModel>(),
    };


    public static VendorViewModel ToViewModel(this Vendor vendor) => new()
    {
        Id = vendor.Id,
        BusinessName = vendor.CompanyName,
        ContactTitle = vendor.ContactPersonName.Title,
        ContactLastName = vendor.ContactPersonName.Surname,
        ContactFirstName = vendor.ContactPersonName.FirstName,
        ContactMobile = vendor.MobilePhoneNumber,
        ContactTelephone = vendor.TelephoneNumber,
        ContactFax = vendor.FascimileNumber,
        ContactEmail = vendor.EmailAddress,
        Address = vendor.PhysicalAddress,
    };

    public static InventoryViewModel ToViewModel(this Inventory inventory) => new()
    {
        Id = inventory.Id,
        Product = inventory.Item.ToViewModel(),
        StockCount = inventory.StockCount,
        MinimumStockCount = inventory.MinimumQuantity,
        MinimumReorderQuantity = inventory.ReOrderQuantity,
        Transactions = [.. inventory.TransactionsHistory.Select(transaction => transaction.ToViewModel())]
    };


    public static InventoryTransactionViewModel ToViewModel(this InventoryTransaction transaction) => new()
    {
        Id = transaction.Id,
        Inventory = transaction.Inventory.ToViewModel(),
        Type = new InventoryTransactionTypeViewModel
        {
            Value = (char)transaction.Type,
            Name = transaction.Type.ToString()
        },
        TimeStamp = transaction.Date,
        Amount = transaction.Amount,

    };
}
