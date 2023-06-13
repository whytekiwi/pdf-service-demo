using System.Net;
using System.Text.Json;
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
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        GeneratePdfRequestObject? body = await JsonSerializer.DeserializeAsync<GeneratePdfRequestObject>(req.Body);
        if (string.IsNullOrEmpty(body?.templateId))
        {
            _logger.LogError("TemplateId is null or empty");
            var res = req.CreateResponse(HttpStatusCode.BadRequest);
            res.WriteString("TemplateId is null or empty");
            return res;
        }

        try
        {
            var pdfStream = await _pdfGenerator.GeneratePdfAsync(body.templateId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/pdf");
            response.Body = pdfStream;
            return response;
        }
        catch (TemplateDoesNotExistException ex)
        {
            _logger.LogError(ex.Message);
            var res = req.CreateResponse(HttpStatusCode.BadRequest);
            res.WriteString(ex.Message);
            return res;
        }
    }
}
