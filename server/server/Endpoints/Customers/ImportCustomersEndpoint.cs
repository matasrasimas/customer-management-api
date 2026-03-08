using server.UseCases;

namespace server.Endpoints.Customers;

public static class ImportCustomersEndpoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/import", async (IImportCustomersUseCase useCase) =>
        {
            IImportCustomersUseCase.Result result = await useCase.ExecuteAsync();
            return result.Success ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
        });
    }
}