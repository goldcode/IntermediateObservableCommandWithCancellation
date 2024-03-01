// See https://aka.ms/new-console-template for more information


using SiLAGen.Client.TestService;
using System.Windows.Input;
using Tecan.Sila2.Cancellation;
using Tecan.Sila2.Cancellation.CancelController;
using Tecan.Sila2.Client;
using Tecan.Sila2.Discovery;

// wait for server to startup.
await Task.Delay(2000);

var connector = new ServerConnector(new DiscoveryExecutionManager());
var discovery = new ServerDiscovery(connector);
var executionManagerFactory = new DiscoveryExecutionManager();

var server = discovery.GetServers(TimeSpan.FromSeconds(3), nic => true).FirstOrDefault() ?? 
    throw new InvalidOperationException("no servers found");

var silaClient = new TestServiceClient(server.Channel, executionManagerFactory);
var cancelControllerClient = new CancelControllerClient(server.Channel, executionManagerFactory);
silaClient.Initialize();

// this command works
var cancelCommand = silaClient.CancelCommandWorks();
cancelControllerClient.CancelCommand((cancelCommand as IClientCommand).CommandId);

// error message as follows:
// Tecan.Sila2.Cancellation.OperationNotSupportedException: 'The provided command does not support cancellation.'

try
{
    var command = silaClient.ObservableIntermediateCommandWithCancellation();
    await Task.Delay(1000);
    cancelControllerClient.CancelCommand((command as IClientCommand).CommandId);
}
catch(OperationNotSupportedException ex) 
{ 
    Console.WriteLine(ex.Message);
}

Console.ReadLine();