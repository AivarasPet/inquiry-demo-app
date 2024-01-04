using Domain.DomainObjects.Inquiries;

namespace Application.Services.InquiriesService
{
    public interface IInquiriesService
    {
        Task<IEnumerable<Inquiry>> SearchAsync(InquirySearchPredicate predicate);
        Task<Inquiry> SaveAsync(Inquiry domainObject);
    }
}
