using System;
using System.Threading.Tasks;


namespace jsolo.simpleinventory.sys.common.interfaces
{
     public interface IIdentityService
    {
        Task<string> GetUserNameAsync(int userId);

        Task<(Result Result, int UserId)> CreateUserAsync(
            int userId,
            string surname,
            string firstNames,
            string userName,
            string email,
            string password
        );

        Task<Result> DeleteUserAsync(int userId);    
    }
}
