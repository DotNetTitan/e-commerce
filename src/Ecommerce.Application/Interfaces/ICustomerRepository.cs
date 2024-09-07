using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid customerId);
        Task<Customer> UpdateAsync(Customer customer);
        Task<Customer> AddAsync(Customer customer);
    }
}