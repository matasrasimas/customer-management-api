using server.Dtos;
using server.Models;

namespace server.Converters;

public class CustomerToCustomerDtoConverter : Converter<Customer, CustomerDto>
{
    public override CustomerDto Convert(Customer input) =>
        new CustomerDto(
            input.Name,
            input.Address,
            input.PostCode);
}