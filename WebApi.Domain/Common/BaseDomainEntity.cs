namespace WebApi.Domain.Common
{

    public abstract class BaseDomainEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}