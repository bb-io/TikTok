using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Tests.TikTokSymphony.Base;

public class CreativeFileManager : IFileManagementClient
{
    public Task<Stream> DownloadAsync(FileReference reference) =>
        Task.FromResult<Stream>(File.OpenRead(Path.Combine(AppContext.BaseDirectory, "TestFiles", "Input", reference.Name)));

    public Task<FileReference> UploadAsync(Stream stream, string contentType, string fileName) =>
        throw new NotSupportedException("Creative downloads return HTTP-backed file references and do not use this test helper for upload.");
}
