using jsolo.simpleinventory.impl.identity;


namespace jsolo.simpleinventory.impl.objects;



public class RefreshToken : core.common.ValueObject
{
    public virtual string Token { get; set; }

    public virtual string JwtId { get; set; }

    public virtual User User { get; set; }

    public virtual DateTime CreationDate { get; set; }

    public virtual DateTime ExpiryDate { get; protected set; }

    public virtual bool? Used { get; protected set; }

    protected RefreshToken() { }


    public RefreshToken(string token, string jwtid, User user, DateTime createdOn, DateTime expiresOn)
    {
        this.Token = token;
        this.JwtId = jwtid;
        this.User = user;
        this.CreationDate = createdOn;
        this.ExpiryDate = expiresOn;
    }

    public virtual RefreshToken SetIsUsed(bool isUsed)
    {
        this.Used = isUsed;

        return this;
    }

    protected override IEnumerable<object> GetEqualityComponents() => new object[]
    {
        Token,
        JwtId,
        User.Id,
    };
}
