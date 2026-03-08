using server.Converters;
using server.Dtos;
using server.Infrastructure;
using server.Models;
using server.UseCases;

namespace server.Extensions;

public static class ServiceExtensions
{
    public static void AddUseCases(this IServiceCollection services)
    {
        services.AddTransient<IImportCustomersUseCase, ImportCustomersUseCase>();
        services.AddTransient<IUpdateCustomersPostCodesUseCase, UpdateCustomersPostCodesUseCase>();
        services.AddTransient<IRetrieveCustomersUseCase, RetrieveCustomersUseCase>();
    }

    public static void AddConverters(this IServiceCollection services)
    {
        services.AddTransient<Converters.Converter<CreateCustomerDto, Customer>, CreateCustomerDtoToCustomerConverter>();
        services.AddTransient<Converters.Converter<Customer, CustomerDto>, CustomerToCustomerDtoConverter>();
    }

    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IPostitService, PostitService>();
        services.AddHttpClient<PostitService>();
    }
}