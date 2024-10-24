using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DDDSample1.Domain.OperationTypes;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Infrastructure.Users
{
    public class UserRepository : BaseRepository<User,Username> ,IUserRepository{
        private readonly DDDSample1DbContext _context;
        public UserRepository(DDDSample1DbContext context):base(context.Users)
        {
           _context=context;
        }

        public async Task<int> CountBackofficeUsersAsync()
        {
            var users = await _context.Users.Where(u => u.Role != Role.PATIENT).ToListAsync();

            return users.Count;
        }

        public async Task<User> GetByEmail(Email email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<List<User>> GetByRole(Role role)
        {
            return await _context.Users.Where(user => user.Role == role).ToListAsync();
        }

        public async Task<User> UpdateAsync(User user) {
            _context.Users.Update(user);
            
            await _context.SaveChangesAsync();

            return user;
        }
    }
}