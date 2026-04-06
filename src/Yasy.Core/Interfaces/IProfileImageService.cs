namespace Yasy.Core.Interfaces;

public interface IProfileImageService
{
    Task<string> SaveAsync(Guid customerId, string fileName, Stream imageStream, CancellationToken ct = default);
    void Delete(string relativePath);
    string GetAbsolutePath(string relativePath);
}
