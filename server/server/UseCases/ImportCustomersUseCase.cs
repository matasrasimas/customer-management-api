using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dtos;
using server.Infrastructure;
using server.Models;

namespace server.UseCases;

public class ImportCustomersUseCase(
    CustomersContext db,
    Converters.Converter<CreateCustomerDto, Customer> converter,
    IFileService fileService) : IImportCustomersUseCase
{
    public async Task<IImportCustomersUseCase.Result> ExecuteAsync()
    {
        DateTime now = DateTime.UtcNow;
        
        if (!fileService.Exists(Constants.CustomersFileName))
            return IImportCustomersUseCase.Result.Fail($"Nerastas failas {Constants.CustomersFileName}");

        List<CreateCustomerDto>? deserializedCustomers = await DeserializeCustomersFromFile();

        if (deserializedCustomers is null)
            return IImportCustomersUseCase.Result.Fail($"{Constants.CustomersFileName} yra tuščias arba turi neteisingą JSON");

        return await SaveCustomers(deserializedCustomers, now);
    }

    private async Task<List<CreateCustomerDto>?> DeserializeCustomersFromFile()
    {
        var serializedCustomers = await fileService.ReadAllTextAsync(Constants.CustomersFileName);
        return JsonSerializer.Deserialize<List<CreateCustomerDto>>(serializedCustomers);
    }

    private async Task<IImportCustomersUseCase.Result> SaveCustomers(List<CreateCustomerDto> input, DateTime now)
    {
        var existingCustomers = await db.Customers
            .Select(c => new { Name = c.Name.ToLower(), Address = c.Address.ToLower() })
            .ToHashSetAsync();

        var newCustomers = converter.Convert(input)
            .Where(c => !existingCustomers.Contains(new { Name = c.Name.ToLower(), Address = c.Address.ToLower() }))
            .ToList();

        var logs = newCustomers.Select(c => new CustomerLog(
            Guid.NewGuid(),
            c.Id,
            CustomerAction.Create,
            now)
        ).ToList();

        db.Customers.AddRange(newCustomers);
        db.CustomerLogs.AddRange(logs);
        await db.SaveChangesAsync();
        
        return IImportCustomersUseCase.Result.Ok();
    }
}