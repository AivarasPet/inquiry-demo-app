using Application.Repositories;
using Domain.DomainObjects.Users;
using Infrastructure.BaseDbContexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly InquiryDbContext _dbContext;

        public UsersRepository(InquiryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _dbContext.UserEntities.FindAsync(id);
            if (entity != null)
            {
                _dbContext.UserEntities.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            IEnumerable<User> users = await SearchAsync(new UserSearchPredicate() { Id = id });
            return users.First();
        }

        public async Task<User> SaveAsync(User user)
        {
            UserEntity entity = null;

            if (user.IsNew)
            {
                entity = new UserEntity
                {
                    Id = user.Id,
                };
                await _dbContext.UserEntities.AddAsync(entity);
            }
            else
            {
                entity = await _dbContext.UserEntities.FirstAsync(e => e.Id == user.Id);
                _dbContext.UserEntities.Update(entity);
            }

            entity.Username = user.Username;
            entity.Password = user.Password;

            await _dbContext.SaveChangesAsync();

            return await GetByIdAsync(entity.Id);
        }

        public async Task<IEnumerable<User>> SearchAsync(UserSearchPredicate predicate)
        {
            IQueryable<UserEntity> queryable = _dbContext.UserEntities
                .Where(q => (predicate.Id == null || predicate.Id == q.Id)
                    && (predicate.Username == null || predicate.Username == q.Username)
                    && (predicate.Password == null || predicate.Password == q.Password));

            return await queryable
                    .Select(entity => new User(entity.Id)
                    {
                        Username = entity.Username,
                        Password = entity.Password
                    })
                    .ToListAsync();
        }
    }
}
