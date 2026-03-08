namespace server.UseCases;

public interface IImportCustomersUseCase
{
    Task<Result> ExecuteAsync();
    
    public record Result(bool Success, string Message)
    {
        public static Result Ok() => new(true, "Klientai buvo sėkmingai suimportuoti");
        public static Result Fail(string message) => new(false, message);
    }
}