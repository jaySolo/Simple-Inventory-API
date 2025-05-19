using jsolo.simpleinventory.core.common;
using jsolo.simpleinventory.core.objects;



namespace jsolo.simpleinventory.core.entities;


///<summary>An item that can be bought and sold.</summary>
public class Product : Entity<int>
{
    #region properties
#nullable enable
    /// <summary>The internally-assigned product number.</summary>
    public virtual string? InternalProductNumber { get; protected set; }

    /// <summary>The manufacturer-assigned product number.</summary>
    public virtual string? ExternalProductNumber { get; protected set; }
#nullable disable

    /// <summary>The name of the product.</summary>
    public virtual string ProductName { get; protected set; }

    /// <summary>The type/catorgory that the procduct is classified as.</summary>
    public virtual ProductType Type { get; protected set; }

    /// <summary>The manufacturer of the procduct.</summary>
    public virtual string Make { get; protected set; }

    /// <summary>A short description of the product.</summary>
    public virtual string Description { get; protected set; }

    /// <summary>The market price or selling value of the product.</summary>
    public virtual Money MarketPrice { get; protected set; }

#nullable enable
    /// <summary>The string value of the product's barcode.</summary>
    public virtual string? BarCode { get; protected set; }
#nullable disable

    /// <summary>A collection of images and videos of the product.</summary>
    public virtual IList<Medium> Gallery { get; protected set; } = new List<Medium>();

    /// <summary>A list of vendors that the product can be purchased from.</summary>
    public virtual IList<Vendor> Suppliers { get; protected set; } = new List<Vendor>();
    #endregion


    #region constructors
    /// <summary>
    /// Creates a new <see cref="Product"/>. Reserved for use by ORM.
    /// </summary>
    protected Product() { }


    /// <summary>
    /// Creates a new <see cref="Product"/>.
    /// </summary>
    /// 
    /// 
    /// <param name="id">The system's identification number of the <see cref="Product"/>.</param>
    /// <param name="intProductNumber">
    /// The internally-assigned product number of the <see cref="Product"/>.
    /// </param>
    /// <param name="extProductNumber">
    /// The manufacturer-assigned product number of the <see cref="Product"/>.
    /// </param>
    /// <param name="productName">The system's idendification number of the <see cref="Product"/>.</param>
    /// <param name="id">The system's idendification number of the <see cref="Product"/>.</param>
    /// 
    /// 
    /// <returns>
    /// A <see cref="Product"/> with all null/empty fields.
    /// </returns>
    public Product(
        int id,
        string intProductNumber,
        string extProductNumber,
        string productName,
        ProductType productType,
        string make,
        string description,
        Money marketPrice,
        DateTime createdOn,
        string creatorId,
#nullable enable
        string? barcode = null,
        IEnumerable<Medium>? media = null,
        IEnumerable<Vendor>? suppliers = null,
        DateTime? lastUpdatedOn = null,
        string? lastUpdaterId = null
#nullable disable
    )
    {
        this.Id = id;

        this.CreatedOn = createdOn;
        this.CreatorId = creatorId;

        this.SetInternalProductNumber(intProductNumber)
            .SetExternalProductNumber(extProductNumber)
            .SetProductName(productName)
            .SetTypeAs(productType)
            .SetMake(make)
            .SetDescription(description)
            .SetMarketPrice(marketPrice)
            .SetBarcode(barcode)
            .SetGalleryAs(media)
            .SetSuppliersAs(suppliers)
            .SetLastModifierAsAt(lastUpdaterId, lastUpdatedOn);
    }
    #endregion


    #region instance methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="intProdNum"></param>
    /// <returns></returns>
    public virtual Product SetInternalProductNumber(string intProdNum)
    {
        // ThrowIfNullOrWhitespaceString(intProdNum, nameof(intProdNum));

        this.InternalProductNumber = intProdNum;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="extProdNum"></param>
    /// <returns></returns>
    public virtual Product SetExternalProductNumber(string extProdNum)
    {
        // ThrowIfNullOrWhitespaceString(extProdNum, nameof(extProdNum));

        this.ExternalProductNumber = extProdNum;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="productName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// The product name is <see cref="null"/> an empty string or only has whitespaces.
    /// </exception>
    public virtual Product SetProductName(string productName)
    {
        ThrowIfNullOrWhitespaceString(productName, nameof(productName));

        this.ProductName = productName;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public virtual Product SetTypeAs(ProductType type)
    {
        this.Type = type;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="make"></param>
    /// <returns></returns>
    public virtual Product SetMake(string make)
    {
        this.Make = make;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public virtual Product SetDescription(string description)
    {
        this.Make = description;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public virtual Product SetMarketPrice(Money price)
    {
        this.MarketPrice = price;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="barcode"></param>
    /// <returns></returns>
    public virtual Product SetBarcode(string barcode)
    {
        this.BarCode = barcode;

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="media"></param>
    /// <returns></returns>
    public virtual Product SetGalleryAs(IEnumerable<Medium> media)
    {
        this.Gallery.Clear();

        foreach (Medium medium in media)
        {
            this.Gallery.Add(medium);
        }

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="suppliers"></param>
    /// <returns></returns>
    public virtual Product SetSuppliersAs(IEnumerable<Vendor> suppliers)
    {
        this.Suppliers.Clear();

        foreach (Vendor supplier in suppliers)
        {
            this.Suppliers.Add(supplier);
        }

        return this;
    }
    #endregion


    #region implementations and overrides
    protected override IEnumerable<object> GetEqualityComponents() => new object[]
    {
        this.InternalProductNumber,
        this.ExternalProductNumber,
        this.BarCode
    };
    #endregion
}
