using Domain.DomainObjects.Inquiries;

namespace Application.Services.InquiriesService
{
    public interface IInquiriesService
    {
        IEnumerable<Inquiry> Search(InquirySearchPredicate predicate);
        Inquiry Save(Inquiry domainObject);
    }
}
