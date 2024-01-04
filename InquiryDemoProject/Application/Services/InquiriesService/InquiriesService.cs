using Application.Repositories;
using Domain.DomainObjects.Inquiries;

namespace Application.Services.InquiriesService
{
    public class InquiriesService : IInquiriesService
    {
        private readonly IInquiriesRepository _inquiriesRepository;

        public InquiriesService(IInquiriesRepository inquiriesRepository)
        {
            _inquiriesRepository = inquiriesRepository;
        }

        public Task<Inquiry> SaveAsync(Inquiry domainObject)
        {
            return _inquiriesRepository.SaveAsync(domainObject);
        }

        public Task<IEnumerable<Inquiry>> SearchAsync(InquirySearchPredicate predicate)
        {
            return _inquiriesRepository.SearchAsync(predicate);
        }
    }
}
