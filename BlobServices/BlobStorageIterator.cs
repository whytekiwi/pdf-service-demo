using Azure.Storage.Blobs;

public interface IBlobStorageIterator
{
    Task<string> ReadBlobAsTextAsync();
}

public class BlobStorageIterator : IBlobStorageIterator
{
    private BlobContainerClient _containerClient;

    public BlobStorageIterator(BlobConfiguration blobConfiguration)
    {
        _containerClient = new BlobServiceClient(blobConfiguration.SasTokenUri)
            .GetBlobContainerClient(blobConfiguration.ContainerName);
    }

    public async Task<string> ReadBlobAsTextAsync()
    {
        try
        {
            using var blobStream = await _containerClient.GetBlobClient("demo.txt").OpenReadAsync();
            using var streamReader = new StreamReader(blobStream);
            return await streamReader.ReadToEndAsync();
        }
        catch
        {
            return "";
        }
    }
}