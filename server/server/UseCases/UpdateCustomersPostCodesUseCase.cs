using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Infrastructure;
using server.Models;

namespace server.UseCases;

public class UpdateCustomersPostCodesUseCase(
    CustomersContext db,
    IPostitService postitService) : IUpdateCustomersPostCodesUseCase
{
    public async Task<IUpdateCustomersPostCodesUseCase.Result> ExecuteAsync()
    {
        DateTime now = DateTime.UtcNow;
        
        var customers = await db.Customers.ToListAsync();
        var failedAddresses = new List<string>();
        
        await UpdateCustomersPostCodes(customers, failedAddresses, now);

        return FormatResultBasedOnFailedAddressesCount(failedAddresses, customers.Count);
    }

    private static IUpdateCustomersPostCodesUseCase.Result FormatResultBasedOnFailedAddressesCount(List<string> failedAddresses, int customersCount)
    {
        if (failedAddresses.Count == 0)
            return IUpdateCustomersPostCodesUseCase.Result.Ok();
        if (failedAddresses.Count == customersCount)
            return IUpdateCustomersPostCodesUseCase.Result.Fail();
        return IUpdateCustomersPostCodesUseCase.Result.Partial(failedAddresses);
    }

    private async Task UpdateCustomersPostCodes(List<Customer> customers, List<string> failedAddresses, DateTime now)
    {
        foreach (var customer in customers)
        {
            string? postCode = await postitService.GetPostCodeAsync(customer.Address);
            if (postCode is null)
            {
                failedAddresses.Add(customer.Address);
                continue;
            }

            var updatedCustomer = customer with { PostCode = postCode };
            db.Entry(customer).CurrentValues.SetValues(updatedCustomer);

            var log = new CustomerLog(
                Guid.NewGuid(),
                updatedCustomer.Id,
                CustomerAction.UpdatePostCode,
                now);
            await db.CustomerLogs.AddAsync(log);
        }

        await db.SaveChangesAsync();
    }
}