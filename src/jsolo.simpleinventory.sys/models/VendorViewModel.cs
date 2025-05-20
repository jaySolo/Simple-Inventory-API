namespace jsolo.simpleinventory.sys.models;



public class VendorViewModel
{
    public int Id { get; set; }

    public string BusinessName { get; set; } = "";

    public string? ContactTitle { get; set; }

    public string? ContactLastName { get; set; }

    public string? ContactFirstName { get; set; }

    public string ContactMobile { get; set; } = "";

    public string ContactTelephone { get; set; } = "";

    public string ContactFax { get; set; } = "";

    public string ContactEmail { get; set; } = "";

    public string Address { get; set; } = "";
}



public class VendorsFilterViewModel : QueryFilterViewModel
{
    public string? BusinessName { get; set; }

    public string? ContactName { get; set; }
    
    public string? ContactMobile { get; set; }
    
    public string? ContactTelephone { get; set; }
    
    public string? ContactFax { get; set; }
    
    public string? ContactEmail { get; set; }
    
    public string? Address { get; set; }
}
