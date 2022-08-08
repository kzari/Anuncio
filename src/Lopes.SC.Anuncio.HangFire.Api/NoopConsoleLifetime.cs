namespace Lopes.SC.Jobs.Api
{
    public class NoopConsoleLifetime : IHostLifetime, IDisposable
    {
        private readonly ILogger<NoopConsoleLifetime> _logger;

        public NoopConsoleLifetime(ILogger<NoopConsoleLifetime> logger)
        {
            _logger = logger;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            Console.CancelKeyPress += OnCancelKeyPressed;
            return Task.CompletedTask;
        }

        private void OnCancelKeyPressed(object? sender, ConsoleCancelEventArgs e)
        {
            _logger.LogInformation("Ctrl+C has been pressed, ignoring.");
            e.Cancel = true;
        }

        public void Dispose()
        {
            Console.CancelKeyPress -= OnCancelKeyPressed;
        }
    }
}
