namespace TimescaleAPI.Application.Interfaces;

public interface IUploadService
{
    public  Task<string> ProcessUpload(Stream stream, string rowFileName, CancellationToken cancellationToken);
}