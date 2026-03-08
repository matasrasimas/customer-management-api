using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using server.Data;
using server.Infrastructure;
using server.Models;
using server.UseCases;

namespace server.Tests.UseCases;

public class UpdateCustomersPostCodesUseCaseTests
{
    private readonly Mock<IPostitService> postitServiceMock = new();
    private readonly CustomersContext db;
    
    private readonly UpdateCustomersPostCodesUseCase useCase;

    public UpdateCustomersPostCodesUseCaseTests()
    {
        var options = new DbContextOptionsBuilder<CustomersContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        db = new CustomersContext(options.Options);
        useCase = new UpdateCustomersPostCodesUseCase(db, postitServiceMock.Object);
    }
    
    [Fact]
    public async Task ExecuteAsync_AllPostCodesRetrievedSuccessfully_ReturnsOkResult()
    {
        Guid customerId1 = Guid.NewGuid();
        Guid customerId2 = Guid.NewGuid();
        
        await db.Customers.AddRangeAsync(
            new Customer(customerId1, "name1", "address1", null),
            new Customer(customerId2, "name2", "address2", null)
        );
        await db.SaveChangesAsync();

        postitServiceMock
            .Setup(p => p.GetPostCodeAsync("address1"))
            .ReturnsAsync("LT-12345");
        postitServiceMock
            .Setup(p => p.GetPostCodeAsync("address2"))
            .ReturnsAsync("LT-12346");

        var result = await useCase.ExecuteAsync();

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Klientų pašto indeksai sėkmingai atnaujinti");
        db.Customers.Single(c => c.Name == "name1").PostCode.Should().Be("LT-12345");
        db.Customers.Single(c => c.Name == "name2").PostCode.Should().Be("LT-12346");

        db.CustomerLogs.Should().HaveCount(2);
        db.CustomerLogs.Should().BeEquivalentTo(
            new List<CustomerLog> 
            { 
                new(Guid.NewGuid(), customerId1, CustomerAction.UpdatePostCode, DateTime.UtcNow), 
                new(Guid.NewGuid(), customerId2, CustomerAction.UpdatePostCode, DateTime.UtcNow) 
            },
            options => options
                .Excluding(log => log.Id)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>());
    }
    
    [Fact]
    public async Task ExecuteAsync_AllPostCodesFail_ReturnsFailResult()
    {
        await db.Customers.AddRangeAsync(
            new Customer(Guid.NewGuid(), "name1", "address1", null),
            new Customer(Guid.NewGuid(), "name2", "address2", null)
        );
        await db.SaveChangesAsync();

        postitServiceMock
            .Setup(p => p.GetPostCodeAsync(It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        var result = await useCase.ExecuteAsync();

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Nepavyko atnaujinti pašto indeksų visiems adresams");
        db.Customers.Select(c => c.PostCode).AsEnumerable().Should().AllBe(null);
    }
    
    [Fact]
    public async Task ExecuteAsync_SomePostCodesRetrievalFail_ReturnsPartialResult()
    {
        await db.Customers.AddRangeAsync(
            new Customer(Guid.NewGuid(), "name1", "address1", null),
            new Customer(Guid.NewGuid(), "name2", "address2", null)
        );
        await db.SaveChangesAsync();

        postitServiceMock
            .Setup(p => p.GetPostCodeAsync("address1"))
            .ReturnsAsync("LT-12345");
        postitServiceMock
            .Setup(p => p.GetPostCodeAsync("address2"))
            .ReturnsAsync((string?)null);

        var result = await useCase.ExecuteAsync();

        result.Success.Should().BeFalse();
        result.Message.Should().Contain("address2");
        db.Customers.Single(c => c.Name == "name1").PostCode.Should().Be("LT-12345");
        db.Customers.Single(c => c.Name == "name2").PostCode.Should().BeNull();
    }
}