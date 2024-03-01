// See https://aka.ms/new-console-template for more information


using SiLAGen.Client.TestService;
using System.Windows.Input;
using Tecan.Sila2.Cancellation.CancelController;
using Tecan.Sila2.Client;
using Tecan.Sila2.Discovery;

await Task.Delay(2000);

var connector = new ServerConnector(new DiscoveryExecutionManager());
var discovery = new ServerDiscovery(connector);
var executionManagerFactory = new DiscoveryExecutionManager();

var server = discovery.GetServers(TimeSpan.FromSeconds(3), nic => true).FirstOrDefault() ?? 
    throw new InvalidOperationException("no servers found");

var silaClient = new TestServiceClient(server.Channel, executionManagerFactory);
var cancelControllerClient = new CancelControllerClient(server.Channel, executionManagerFactory);
silaClient.Initialize();

var cancelCommand = silaClient.CancelCommandWorks();
cancelControllerClient.CancelCommand((cancelCommand as IClientCommand).CommandId);

var command = silaClient.ObservableIntermediateCommandWithCancellation();
await Task.Delay(1000);
cancelControllerClient.CancelCommand((command as IClientCommand).CommandId);

//var commandExecution = silaClient.SomeCommand();
//await Task.Delay(2000);
//cancelControllerClient.CancelCommand((commandExecution as IClientCommand ?? throw new NullReferenceException(nameof(commandExecution))).CommandId);
//await commandExecution.Response;