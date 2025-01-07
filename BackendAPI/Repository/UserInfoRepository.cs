using BackendAPI.Interface;
using BackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendAPI.Repository
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly DatabaseContext _context;

        public UserInfoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<UserInfo> AddUserAsync(UserInfo userInfo)
        {
            await _context.UserInfos.AddAsync(userInfo);
            await _context.SaveChangesAsync();
            return userInfo;
        }

        public async Task<UserInfo> GetUserByEmailAsync(string email)
        {
            return await _context.UserInfos.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<UserInfo>> GetAllUsersAsync()
        {
            return await _context.UserInfos.ToListAsync();
        }
    }
}
