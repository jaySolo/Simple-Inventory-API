using System.ComponentModel.DataAnnotations;

namespace jsolo.simpleinventory.sys.models;



public class ProductTypeViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [Required]
    public required string Name { get; set; }


    public string? Description { get; set; }
}


public class ProductTypesFilterViewModel : QueryFilterViewModel
{
    public string Name { get; set; } = "";
}