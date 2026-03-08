using FluentAssertions;
using server.Converters;
using server.Dtos;
using server.Models;

namespace server.Tests.Converters;

public class CustomerToCustomerDtoConverterTests
{
    private readonly CustomerToCustomerDtoConverter converter;

    public CustomerToCustomerDtoConverterTests()
    {
        converter = new CustomerToCustomerDtoConverter();
    }

    [Fact]
    public void Convert_ReturnsCustomerDto()
    {
        Customer input = new Customer(Guid.NewGuid(), "name", "address", "123");
        CustomerDto expected = new CustomerDto("name", "address", "123");
        
        CustomerDto output = converter.Convert(input);

        output.Should().Be(expected);
    }
}