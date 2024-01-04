using Domain.DomainObjects.Users;

namespace Application.Services.UserService
{
    public interface IUsersService
    {
        public Task<(bool isValid, User user)> ValidateCredentialsAsync(string username, string password);
        public Task<IEnumerable<User>> SearchAsync(UserSearchPredicate predicate);
        public Task<User> SaveAsync(User domainObject);
    }
}
