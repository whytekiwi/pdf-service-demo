using Azure;
using Azure.Storage.Blobs;

public interface IBlobStorageIterator
{
    Task<string> ReadBlobAsTextAsync(string blobPath);
}

public class BlobStorageIterator : IBlobStorageIterator
{
    private BlobContainerClient _containerClient;

    public BlobStorageIterator(BlobConfiguration blobConfiguration)
    {
        _containerClient = new BlobServiceClient(blobConfiguration.SasTokenUri)
            .GetBlobContainerClient(blobConfiguration.ContainerName);
    }

    public async Task<string> ReadBlobAsTextAsync(string blobPath)
    {
        try
        {
            using var blobStream = await _containerClient.GetBlobClient(blobPath).OpenReadAsync();
            using var streamReader = new StreamReader(blobStream);
            return await streamReader.ReadToEndAsync();
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == "BlobNotFound")
        {
            throw new TemplateDoesNotExistException($"Could not find specified template {blobPath}");
        }
    }
}

public class TemplateDoesNotExistException : Exception
{
    public TemplateDoesNotExistException(string message) : base(message)
    {
    }
}