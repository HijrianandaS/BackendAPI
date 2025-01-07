using BackendAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendAPI.Interface
{
    public interface IUserInfoRepository
    {
        Task<UserInfo> AddUserAsync(UserInfo userInfo);
        Task<UserInfo> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserInfo>> GetAllUsersAsync();
    }
}
