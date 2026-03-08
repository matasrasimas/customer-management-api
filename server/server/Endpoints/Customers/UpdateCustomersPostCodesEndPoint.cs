using server.UseCases;

namespace server.Endpoints.Customers;

public static class UpdateCustomersPostCodesEndPoint
{
    public static void Map(RouteGroupBuilder group)
    {
        group.MapPut("/post-codes", async (IUpdateCustomersPostCodesUseCase useCase) =>
        {
            IUpdateCustomersPostCodesUseCase.Result result = await useCase.ExecuteAsync();
            return result.Success ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
        });
    }
}