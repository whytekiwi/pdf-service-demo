using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class GeneratePdf
{
    private readonly ILogger _logger;
    private readonly IPdfGenerator _pdfGenerator;

    public GeneratePdf(ILoggerFactory loggerFactory, IPdfGenerator pdfGenerator)
    {
        _logger = loggerFactory.CreateLogger<GeneratePdf>();
        _pdfGenerator = pdfGenerator;
    }

    [Function("GeneratePdf")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        var filePath = "C:\\Repos\\PDFServiceDemo\\Templates\\PrintableCalendar.html";
        var pdfStream = await _pdfGenerator.GeneratePdfAsync(filePath);

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/pdf");
        response.Body = pdfStream;

        return response;
    }
}
