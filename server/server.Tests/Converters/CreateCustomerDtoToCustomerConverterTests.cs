using FluentAssertions;
using server.Converters;
using server.Dtos;
using server.Models;

namespace server.Tests.Converters;

public class CreateCustomerDtoToCustomerConverterTests
{
    private readonly CreateCustomerDtoToCustomerConverter converter = new();

    [Fact]
    public void Convert_InputContainsFieldsWithEmptySpaces_ReturnsTrimmedFields()
    {
        CreateCustomerDto input = new CreateCustomerDto("  name  ", "  address  ", "  postCode  ");
        Customer expected = new Customer(Guid.NewGuid(), "name", "address", "postCode");

        Customer output = converter.Convert(input);

        output.Should().BeEquivalentTo(expected, options => options.Excluding(c => c.Id));
    }
    
    [Fact]
    public void Convert_PostCodeContainsOnlyWhiteSpaces_ReturnsPostCodeAsNull()
    {
        CreateCustomerDto input = new CreateCustomerDto("name", "address", "    "); ;

        Customer output = converter.Convert(input);
        
        output.PostCode.Should().BeNull();
    }
}