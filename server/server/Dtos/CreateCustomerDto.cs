using System.ComponentModel.DataAnnotations;

namespace server.Dtos;

public record CreateCustomerDto(
    [Required] string Name,
    [Required] string Address, 
    string? PostCode);