using Domain.DomainObjects.Inquiries;

namespace Application.Repositories
{
    public interface IInquiriesRepository : IRepository<Inquiry, InquirySearchPredicate>
    {
    }
}
