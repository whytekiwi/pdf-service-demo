using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class ReadBlob
{
    private readonly ILogger _logger;
    private readonly IBlobStorageIterator _blobStorageIterator;

    public ReadBlob(ILoggerFactory loggerFactory, IBlobStorageIterator blobStorageIterator)
    {
        _logger = loggerFactory.CreateLogger<ReadBlob>();
        _blobStorageIterator = blobStorageIterator;
    }

    [Function("ReadBlob")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        var data = await _blobStorageIterator.ReadBlobAsTextAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString(data);

        return response;
    }
}
