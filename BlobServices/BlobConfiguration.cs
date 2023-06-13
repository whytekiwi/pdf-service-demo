public class BlobConfiguration
{
    public Uri? SasTokenUri { get; set; }
    public string? ContainerName { get; set; }

    public BlobConfiguration()
    {
        string? sasTokenUri = Environment.GetEnvironmentVariable("BlobSasTokenUri", EnvironmentVariableTarget.Process);
        string? containerName = Environment.GetEnvironmentVariable("BlobContainerName", EnvironmentVariableTarget.Process);


        if (string.IsNullOrEmpty(sasTokenUri))
        {
            throw new BlobConfigurationException("Did not set SasTokenUri in configuration");
        }
        if (string.IsNullOrEmpty(containerName))
        {
            throw new BlobConfigurationException("Did not set ContainerName in configuration");
        }

        this.SasTokenUri = new Uri(sasTokenUri);
        this.ContainerName = containerName;
    }
}

public class BlobConfigurationException : Exception
{
    public BlobConfigurationException(string message) : base(message)
    {
    }
}