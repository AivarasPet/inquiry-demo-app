using Domain.DomainObjects.Users;

namespace Application.Repositories
{
    public interface IUsersRepository : IRepository<User, UserSearchPredicate>
    {
    }
}
