using Ecommerce.Domain.Entities;

namespace Ecommerce.Domain.ValueObjects
{
    public class Address
    {
        public required string Building { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }
        public City? City { get; set; }
        public required Guid CityId { get; set; }
        public State? State { get; set; }
        public required Guid StateId { get; set; }
        public Country? Country { get; set; }
        public required Guid CountryId { get; set; }

        public Address(string street, Guid cityId, Guid stateId, Guid countryId, string postalCode)
        {
            Street = street;
            CityId = cityId;
            StateId = stateId;
            CountryId = countryId;
            PostalCode = postalCode;
        }
    }
}