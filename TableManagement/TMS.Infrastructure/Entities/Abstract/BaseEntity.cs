using System.ComponentModel.DataAnnotations;

namespace TMS.Infrastructure.Entities.Abstract
{
    public abstract class BaseEntity<TKey>
    {
        [Key]
        public int Id { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset? LastChangeDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
