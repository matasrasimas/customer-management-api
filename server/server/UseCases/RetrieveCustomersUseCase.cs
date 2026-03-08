using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dtos;
using server.Models;

namespace server.UseCases;

public class RetrieveCustomersUseCase(
    CustomersContext db,
    server.Converters.Converter<Customer, CustomerDto> converter) : IRetrieveCustomersUseCase
{
    public async Task<List<CustomerDto>> ExecuteAsync()
    {
        return await db.Customers.Select(c => converter.Convert(c)).ToListAsync();
    }
}