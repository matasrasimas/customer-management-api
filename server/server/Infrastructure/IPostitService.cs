namespace server.Infrastructure;

public interface IPostitService
{
    Task<string?> GetPostCodeAsync(string address);
}