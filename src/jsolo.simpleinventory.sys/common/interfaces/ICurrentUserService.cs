using System;


namespace jsolo.simpleinventory.sys.common.interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        
        string UserName { get; }
    }
}
