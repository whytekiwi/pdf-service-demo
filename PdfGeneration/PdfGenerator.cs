using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;

public interface IPdfGenerator
{
    Task<Stream> GeneratePdfAsync(string templateLocation);
}

public class PdfGenerator : IPdfGenerator
{
    private IBlobStorageIterator _blobStorageIterator;

    public PdfGenerator(IBlobStorageIterator blobStorageIterator)
    {
        _blobStorageIterator = blobStorageIterator;
    }

    public async Task<Stream> GeneratePdfAsync(string templateLocation)
    {
        var template = await ParseTemplateFromData(templateLocation);
        HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);
        PdfDocument document = htmlConverter.Convert(template, string.Empty);
        Stream memStream = new MemoryStream();
        document.Save(memStream);
        memStream.Seek(0, SeekOrigin.Begin);
        return memStream;
    }

    private Task<string> ParseTemplateFromData(string templateFileLocation)
    {
        return _blobStorageIterator.ReadBlobAsTextAsync(templateFileLocation);
    }
}