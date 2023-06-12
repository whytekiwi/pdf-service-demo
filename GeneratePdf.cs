using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace PdfService.Demo
{
    public class GeneratePdf
    {
        private readonly ILogger _logger;
        private readonly IPdfGeneator _pdfGeneator;

        public GeneratePdf(ILoggerFactory loggerFactory, IPdfGeneator pdfGeneator)
        {
            _logger = loggerFactory.CreateLogger<GeneratePdf>();
            _pdfGeneator = pdfGeneator;
        }

        [Function("GeneratePdf")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var filePath = "C:\\Repos\\PDFServiceDemo\\Templates\\PrintableCalendar.html";
            var pdfStream = await _pdfGeneator.GeneratePdfAsync(filePath);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/pdf");
            response.Body = pdfStream;

            return response;
        }
    }
}
