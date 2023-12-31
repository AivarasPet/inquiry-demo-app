using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class BaseDomainEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

    }
}
