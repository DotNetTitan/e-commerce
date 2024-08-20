namespace Ecommerce.Domain.Entities
{
    public class City : BaseEntity
    {
        public City()
        {
            CityId = Guid.NewGuid();
        }

        public Guid CityId { get; }
        public required string Name { get; set; }
        public required Guid StateId { get; set; }
        public State? State { get; set; }
    }
}