namespace server.Infrastructure;

public class FileService : IFileService
{
    public bool Exists(string path) => File.Exists(path);

    public Task<string> ReadAllTextAsync(string path) => File.ReadAllTextAsync(path);
}