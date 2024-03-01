using Sila.Contracts;
using Tecan.Sila2;

namespace Implementation
{
    public class TestService : ITestService
    {

        public void Initialize()
        {
            
        }

        public Task SomeCommand(CancellationToken cancellationToken) => Task.Delay(int.MaxValue, cancellationToken);

        public async Task CancelCommandWorks(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(int.MaxValue, cancellationToken);
            }
            catch(TaskCanceledException ex)
            {
                Console.Write(ex.Message);
            }
        }

        public IIntermediateObservableCommand<string> ObservableIntermediateCommandWithCancellation()
        {
            return new ObservableIntermediateCommand();
        }
    }

    class ObservableIntermediateCommand : ObservableIntermediatesCommand<string>
    {
        public override async Task Run()
        {
            //PushStateUpdate(0, TimeSpan.MaxValue, CommandState.Running);   
            try
            {
                await Task.Delay(2000, CancellationToken);
                PushIntermediate("Hello");
                await Task.Delay(2000, CancellationToken);
                PushIntermediate("world");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
