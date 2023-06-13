using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

public interface IPdfGenerator
{
    Task<Stream> GeneratePdfAsync(string templateLocation);
}

public class PdfGenerator : IPdfGenerator
{
    public PdfGenerator() { }

    public async Task<Stream> GeneratePdfAsync(string templateLocation)
    {
        var template = await ParseTemplateFromData(templateLocation);
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
        PdfDocument document = htmlConverter.Convert(template, Path.GetDirectoryName(templateLocation));
        Stream memStream = new MemoryStream();
        document.Save(memStream);
        memStream.Seek(0, SeekOrigin.Begin);
        return memStream;
    }

    private Task<string> ParseTemplateFromData(string templateFileLocation)
    {
        // TODO interpolate data into template
        return File.ReadAllTextAsync(templateFileLocation);
    }
}