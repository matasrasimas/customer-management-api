using server.Dtos;
using server.UseCases;

namespace server.Endpoints.Customers;

public static class RetrieveCustomersEndPoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", async (IRetrieveCustomersUseCase useCase) =>
        {
            List<CustomerDto> result = await useCase.ExecuteAsync();
            return Results.Ok(result);
        });
    }
}