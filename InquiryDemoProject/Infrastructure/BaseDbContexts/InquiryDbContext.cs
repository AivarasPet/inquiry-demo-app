using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BaseDbContexts
{
    public class InquiryDbContext : DbContext
    {
        public InquiryDbContext(DbContextOptions<InquiryDbContext> options) : base(options) { }
        public DbSet<InquiryEntity> InquiryEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }
    }
}
