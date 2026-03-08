using server.Endpoints.Customers;

namespace server.Endpoints;

public static class CustomersEndPoints
{
    public static void MapCustomersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/customers");
        
        ImportCustomersEndpoint.Map(group);
        UpdateCustomersPostCodesEndPoint.Map(group);
        RetrieveCustomersEndPoint.Map(group);
    }
}