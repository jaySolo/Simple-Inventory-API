using jsolo.simpleinventory.sys.common.interfaces;


namespace jsolo.simpleinventory.web.Services;



public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        // string _usrId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        string _usrName = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;

        UserId = int.TryParse(_usrName, out int usrId) ? usrId : -1;

        UserName = _usrName;
    }
    

    public int UserId { get; }
    

    public string UserName { get; }
}
