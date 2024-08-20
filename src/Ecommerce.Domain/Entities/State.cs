namespace Ecommerce.Domain.Entities
{
    public class State : BaseEntity
    {
        public State()
        {
            StateId = Guid.NewGuid();
            Cities = new List<City>();
        }

        public Guid StateId { get; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required Guid CountryId { get; set; }
        public Country? Country { get; set; }
        public ICollection<City> Cities { get; }
    }
}