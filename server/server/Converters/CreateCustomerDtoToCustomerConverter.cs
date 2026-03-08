using server.Dtos;
using server.Models;

namespace server.Converters;

public class CreateCustomerDtoToCustomerConverter : Converter<CreateCustomerDto, Customer>
{
    public override Customer Convert(CreateCustomerDto input) => new Customer(
        Guid.NewGuid(),
        input.Name.Trim(),
        input.Address.Trim(),
        string.IsNullOrWhiteSpace(input.PostCode) ? null : input.PostCode.Trim()
        );
}