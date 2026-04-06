using Yasy.Core.Interfaces;

namespace Yasy.Infrastructure.Services;

public class ProfileImageService : IProfileImageService
{
    private readonly string _basePath;

    public ProfileImageService(string basePath)
    {
        _basePath = Path.Combine(basePath, "profiles");
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Guid customerId, string fileName, Stream imageStream, CancellationToken ct = default)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        var storedName = $"{customerId}{extension}";
        var fullPath = Path.Combine(_basePath, storedName);

        // Remove old image with different extension
        DeleteExisting(customerId);

        await using var fileStream = new FileStream(fullPath, FileMode.Create);
        await imageStream.CopyToAsync(fileStream, ct);

        return $"profiles/{storedName}";
    }

    public void Delete(string relativePath)
    {
        var fullPath = Path.Combine(Path.GetDirectoryName(_basePath)!, relativePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    public string GetAbsolutePath(string relativePath)
    {
        return Path.Combine(Path.GetDirectoryName(_basePath)!, relativePath);
    }

    private void DeleteExisting(Guid customerId)
    {
        foreach (var file in Directory.GetFiles(_basePath, $"{customerId}.*"))
        {
            File.Delete(file);
        }
    }
}
