using jsolo.simpleinventory.core.common;



namespace jsolo.simpleinventory.core.objects;


public class Money : ValueObject
{
    #region properites
    public decimal Amount { get; protected set; }

    public Currency Currency { get; protected set; }
    #endregion


    #region constructors
    protected Money() { }


    public Money(decimal amount, Currency currency)
    {
        this.Amount = amount;
        
        this.Currency = currency;
    }
    #endregion

    #region  implementatiions and overrides
    protected sealed override IEnumerable<object> GetEqualityComponents() => new object[]
    {
        this.Amount,
        this.Currency.Code
    };
    #endregion
}

