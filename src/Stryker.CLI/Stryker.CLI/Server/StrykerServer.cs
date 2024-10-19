using System;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using StreamJsonRpc;
using Stryker.Abstractions.Options;
using Stryker.Core.Initialisation;

namespace Stryker.CLI.Server;

#nullable enable

public class StrykerServer
{
    const int Port = 12345;

    private readonly IConfigBuilder _configBuilder;
    private readonly IProjectOrchestrator _projectOrchestrator;
    private readonly IFileSystem _fileSystem;
    private readonly IStrykerInputs _inputs;

    public StrykerServer(
        IFileSystem fileSystem,
        IStrykerInputs inputs,
        IConfigBuilder configBuilder = null,
        IProjectOrchestrator? projectOrchestrator = null)
    {
        // TODO: Use filesystem and inputs.
        _fileSystem = fileSystem;
        _inputs = inputs;
        _configBuilder = configBuilder ?? new ConfigBuilder();
        _projectOrchestrator = projectOrchestrator ?? new ProjectOrchestrator();
    }

    public async Task RunAsync(CancellationToken stoppingToken)
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();

        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        // Output port to stdout so caller can know where to find the server.
        var output = new { port };
        var jsonOutput = JsonSerializer.Serialize(output);
        Console.WriteLine(jsonOutput);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await AcceptClientAsync(listener, stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Don't throw when server is being stopped.
        }

        listener.Stop();
        listener.Dispose();
    }

    private async Task AcceptClientAsync(TcpListener listener, CancellationToken stoppingToken)
    {
        var client = await listener.AcceptTcpClientAsync();
        var stream = client.GetStream();

        var transformer = (string name) => $"{char.ToLower(name[0])}{name[1..]}";
        var rpcOptions = new JsonRpcProxyOptions
        {
            EventNameTransform = transformer,
            MethodNameTransform = transformer
        };
        var rpc = JsonRpc.Attach(stream, new RpcController(_fileSystem, _inputs, _configBuilder, _projectOrchestrator));
        var traceListener = new ConsoleTraceListener(true);

        rpc.TraceSource.Listeners.Add(traceListener);

        // TODO: improve cancellation.
        stoppingToken.Register(() =>
        {
            client.Close();
            rpc.Completion.ConfigureAwait(false).GetAwaiter().GetResult();
        });
    }
}
