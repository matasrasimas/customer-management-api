namespace server.Infrastructure;

public interface IFileService
{
    bool Exists(string path);
    
    Task<string> ReadAllTextAsync(string path);
}