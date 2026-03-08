using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using server.Data;
using server.Dtos;
using server.Infrastructure;
using server.Models;
using server.UseCases;

namespace server.Tests.UseCases;

public class ImportCustomersUseCaseTests
{
    private readonly Mock<IFileService> fileServiceMock = new();
    private readonly Mock<server.Converters.Converter<CreateCustomerDto, Customer>> converterMock = new();
    private readonly CustomersContext db;
    
    private readonly ImportCustomersUseCase useCase;

    public ImportCustomersUseCaseTests()
    {
        var options = new DbContextOptionsBuilder<CustomersContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        db = new CustomersContext(options.Options);
        useCase = new ImportCustomersUseCase(db, converterMock.Object, fileServiceMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_FileNotFound_ReturnsFailResult()
    {
        fileServiceMock
            .Setup(f => f.Exists(Constants.CustomersFileName))
            .Returns(false);

        var output = await useCase.ExecuteAsync();

        output.Success.Should().BeFalse();
        output.Message.Should().Contain("Nerastas failas");
    }

    [Fact]
    public async Task ExecuteAsync_FileIsNull_ReturnsFailResult()
    {
        fileServiceMock
            .Setup(f => f.Exists(Constants.CustomersFileName))
            .Returns(true);
        fileServiceMock
            .Setup(f => f.ReadAllTextAsync(Constants.CustomersFileName))
            .ReturnsAsync("null");

        var output = await useCase.ExecuteAsync();
        
        output.Success.Should().BeFalse();
        output.Message.Should().Contain("yra tuščias arba turi neteisingą JSON");
    }

    [Fact]
    public async Task ExecuteAsync_ValidFile_ImportOnlyNewCustomers()
    {
        var existingCustomer = new Customer(Guid.NewGuid(), "existingName", "existingAddress", "");
        db.Customers.Add(existingCustomer);
        await db.SaveChangesAsync();

        var dtos = new List<CreateCustomerDto>
        {
            new("newName", "newAddress", ""),
            new("existingName", "existingAddress", "")
        };

        Guid customerId1 = Guid.NewGuid();
        Guid customerId2 = Guid.NewGuid();
        var convertedCustomers = new List<Customer>
        {
            new(customerId1, "newName", "newAddress", ""),
            new(customerId2, "existingName", "existingAddress", "")
        };

        fileServiceMock
            .Setup(f => f.Exists(Constants.CustomersFileName))
            .Returns(true);
        fileServiceMock
            .Setup(f => f.ReadAllTextAsync(Constants.CustomersFileName))
            .ReturnsAsync(JsonSerializer.Serialize(dtos));

        converterMock
            .Setup(c => c.Convert(dtos[0]))
            .Returns(convertedCustomers[0]);
        converterMock
            .Setup(c => c.Convert(dtos[1]))
            .Returns(convertedCustomers[1]);

        var output = await useCase.ExecuteAsync();

        output.Success.Should().BeTrue();
        db.Customers.Count().Should().Be(2);
        output.Message.Should().Contain("Klientai buvo sėkmingai suimportuoti");
        
        db.CustomerLogs.Should().HaveCount(1);
        db.CustomerLogs.Should().BeEquivalentTo(
            new List<CustomerLog> 
            { 
                new(Guid.NewGuid(), customerId1, CustomerAction.Create, DateTime.UtcNow)
            },
            options => options
                .Excluding(log => log.Id)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());
    }
}