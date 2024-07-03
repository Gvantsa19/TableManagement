namespace TMS.Infrastructure.Entities.Abstract
{
    public abstract class BaseDeletableModel<TKey> : BaseEntity<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedOn { get; set; }
    }
}
