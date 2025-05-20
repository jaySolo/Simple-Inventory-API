using System.ComponentModel.DataAnnotations;

namespace jsolo.simpleinventory.sys.models;



public class ProductViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public int Id { get; set; }

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
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Make { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; set; }

    public MoneyViewModel? MarketValue { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Barcode { get; set; }

    //gallery

    /// <summary>
    /// 
    /// </summary>
    public List<VendorViewModel> Suppliers = [];
}

public class ProductsFilterViewModel : QueryFilterViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public string InternalProductNumber { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    public string ExternalProductNumber { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    public string Type { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    public string Make { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    public string Barcode { get; set; } = "";
}