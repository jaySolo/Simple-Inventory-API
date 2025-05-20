using System.ComponentModel.DataAnnotations;

namespace jsolo.simpleinventory.sys.models;



public class CurrencyViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [StringLength(maximumLength: 3, MinimumLength = 3)]
    public required string Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [StringLength(maximumLength: 5)]
    public string? Symbol { get; set; }


    public string? Description { get; set; }
}


public class CurrenciesFilterViewModel : QueryFilterViewModel
{
    public string Code { get; set; } = "";

    public string Name { get; set; } = "";
    
    public string Symbol { get; set; } = "";
}