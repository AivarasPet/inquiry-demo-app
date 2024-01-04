namespace Application.Repositories
{
    public interface IRepository<T, TPredicate> where T : class
                                               where TPredicate : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<T>> SearchAsync(TPredicate predicate);
        Task<T> SaveAsync(T domainObject);
    }
}
