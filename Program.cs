using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<BlobConfiguration>();
        services.AddSingleton<IPdfGenerator, PdfGenerator>();
        services.AddSingleton<IBlobStorageIterator, BlobStorageIterator>();

    })
    .Build();

host.Run();
