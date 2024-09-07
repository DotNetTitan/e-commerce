namespace Ecommerce.Domain.ValueObjects
{
    public record Address(string Building, string Street, string PostalCode, string City, string State, string Country);
}