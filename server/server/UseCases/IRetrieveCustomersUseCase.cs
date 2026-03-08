using server.Dtos;

namespace server.UseCases;

public interface IRetrieveCustomersUseCase
{
    Task<List<CustomerDto>> ExecuteAsync();
}