namespace server.Models;

public record CustomerLog(
    Guid Id,
    Guid CustomerId,
    CustomerAction Action,
    DateTime PerformedAt
    );