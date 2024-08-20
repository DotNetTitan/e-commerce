namespace Ecommerce.Domain.Entities
{
    public class Country : BaseEntity
    {
        public Country()
        {
            CountryId = Guid.NewGuid();
            States = new List<State>();
        }

        public Guid CountryId { get; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public ICollection<State> States { get; }
    }
}