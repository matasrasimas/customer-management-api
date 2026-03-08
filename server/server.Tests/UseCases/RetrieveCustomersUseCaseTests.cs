using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using server.Data;
using server.Dtos;
using server.Models;
using server.UseCases;

namespace server.Tests.UseCases;

public class RetrieveCustomersUseCaseTests
{
    private readonly Mock<server.Converters.Converter<Customer, CustomerDto>> converterMock = new();
    private readonly CustomersContext db;

    private readonly RetrieveCustomersUseCase useCase;

    public RetrieveCustomersUseCaseTests()
    {
        var options = new DbContextOptionsBuilder<CustomersContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        
        db = new CustomersContext(options.Options);
        useCase = new RetrieveCustomersUseCase(db, converterMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsCustomersFromDb()
    {
        Customer customerFromDb = new Customer(Guid.NewGuid(), "name", "address", "123");
        await db.Customers.AddAsync(customerFromDb);
        await db.SaveChangesAsync();
        
        CustomerDto convertedCustomer = new CustomerDto("name", "address", "123");
        converterMock
            .Setup(c => c.Convert(customerFromDb))
            .Returns(convertedCustomer);

        var output = await useCase.ExecuteAsync();
        
        output.Count.Should().Be(1);
        output.Should().Contain(convertedCustomer);
    }
}