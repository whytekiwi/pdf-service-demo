using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

public interface IPdfGeneator
{
    Task<Stream> GeneratePdfAsync(string templateLocation);
}

public class PdfGeneator : IPdfGeneator
{
    public PdfGeneator() { }

    public async Task<Stream> GeneratePdfAsync(string templateLocation)
    {
        var template = await ParseTempalteFromData(templateLocation);
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
        PdfDocument document = htmlConverter.Convert(template, Path.GetDirectoryName(templateLocation));
        Stream memStream = new MemoryStream();
        document.Save(memStream);
        memStream.Seek(0, SeekOrigin.Begin);
        return memStream;
    }

    private Task<string> ParseTempalteFromData(string templateFileLocation)
    {
        // TODO interpolate data into template
        return File.ReadAllTextAsync(templateFileLocation);
    }
}