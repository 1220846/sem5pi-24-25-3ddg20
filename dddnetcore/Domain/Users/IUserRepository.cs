using System.Threading.Tasks;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Users
{
    public interface IUserRepository : IRepository<User, Username>
    {
        Task<int> CountBackofficeUsersAsync();
    }
}