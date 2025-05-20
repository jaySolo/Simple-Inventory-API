using Microsoft.AspNetCore.Identity;

using jsolo.simpleinventory.impl.identity;


namespace jsolo.simpleinventory.impl.providers;

public class FirstTimeLoginTokenProvider : TotpSecurityStampBasedTokenProvider<User>
{

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
    {
        return Task.FromResult(false);
    }



    public override async Task<string> GetUserModifierAsync(string purpose, UserManager<User> manager, User user)
    {
        var emailAddress = await manager.GetEmailAsync(user);
        return $"PasswordlessLogin:{purpose}:{emailAddress}";
    }
}