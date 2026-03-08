namespace server.Models;

public record Customer(Guid Id, string Name, string Address, string? PostCode);
