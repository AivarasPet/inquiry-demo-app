using Application.Repositories;
using Domain.DomainObjects.Inquiries;
using Infrastructure.BaseDbContexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories
{
    public class InquiriesRepository : IInquiriesRepository
    {

        private readonly InquiryDbContext _dbContext;

        public InquiriesRepository(InquiryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Inquiry> GetByIdAsync(Guid id)
        {
            IEnumerable<Inquiry> inquiries = await SearchAsync(new InquirySearchPredicate() { Id = id });
            return inquiries.First();
        }


        public async Task<Inquiry> SaveAsync(Inquiry domainObject)
        {
            InquiryEntity entity = null;

            if (domainObject.IsNew)
            {
                entity = new InquiryEntity
                {
                    Id = domainObject.Id,
                };
            }
            else
            {
                entity = _dbContext
                    .InquiryEntities
                    .First(e => e.Id == domainObject.Id);
            }

            entity.Status = domainObject.Status;
            entity.Type = domainObject.Type;
            entity.Message = domainObject.Message;
            entity.UserId = domainObject.UserId;
            entity.CreationDate = domainObject.CreationDate;

            if (domainObject.IsNew)
            {
                _dbContext
                    .InquiryEntities
                    .Add(entity);
            }
            else
            {
                _dbContext
                     .InquiryEntities
                     .Update(entity);
            }

            await _dbContext
                .SaveChangesAsync();

            return GetByIdAsync(entity.Id).Result;
        }

        public async Task<IEnumerable<Inquiry>> SearchAsync(InquirySearchPredicate predicate)
        {
            IQueryable<InquiryEntity> queryable = _dbContext
                .InquiryEntities
                .Where(q => (predicate.Id == null || predicate.Id == q.Id)
                    && (predicate.InquiryStatus == null || predicate.InquiryStatus.Value == q.Status)
                    && (predicate.UserId == null || predicate.UserId.Value == q.UserId)
                    && (predicate.InquiryType == null || predicate.InquiryType.Value == q.Type)
                );

            return queryable
                    .Select(entity => new Inquiry(entity.Id, entity.CreationDate, entity.Type, entity.Status, entity.Message, entity.UserId))
                    .ToList();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
