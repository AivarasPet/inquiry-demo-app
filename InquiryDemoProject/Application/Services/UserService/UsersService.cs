using Application.Repositories;
using Domain.DomainObjects.Users;

namespace Application.Services.UserService
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _userRepository;

        public UsersService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<IEnumerable<User>> SearchAsync(UserSearchPredicate predicate)
        {
            return _userRepository.SearchAsync(predicate);
        }

        public Task<User> SaveAsync(User domainObject)
        {
            if (domainObject.IsNew)
            {
                domainObject.Password = HashPassword(domainObject.Password);
            }
            return _userRepository.SaveAsync(domainObject);
        }

        public async Task<(bool isValid, User user)> ValidateCredentialsAsync(string username, string password)
        {
            IEnumerable<User> users = await _userRepository.SearchAsync(new UserSearchPredicate()
            {
                Username = username,
            });

            var user = users.FirstOrDefault();

            bool isValid = user != null && VerifyPassword(password, user.Password);

            return (isValid, user);
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }
}
