using Microsoft.AspNetCore.Identity;


namespace jsolo.simpleinventory.impl.identity
{
     public class UserLogin : IdentityUserLogin<long>
    {
        public virtual User User { get; protected set; }
    }
}