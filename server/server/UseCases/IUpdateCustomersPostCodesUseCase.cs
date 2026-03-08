namespace server.UseCases;

public interface IUpdateCustomersPostCodesUseCase
{
    Task<Result> ExecuteAsync();
    
    public record Result(bool Success, string Message)
    {
        public static Result Ok() => new(true, "Klientų pašto indeksai sėkmingai atnaujinti");
        
        public static Result Partial(List<string> failedAddresses) => new(false,
            $"Nepavyko atnaujinti pašto indeksų šiems adresams: {string.Join(", ", failedAddresses)}");

        public static Result Fail() => new(false,
            "Nepavyko atnaujinti pašto indeksų visiems adresams");
    }
}