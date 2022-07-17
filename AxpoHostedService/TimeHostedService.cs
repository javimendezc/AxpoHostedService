using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;


namespace AxpoHostedService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly IWorker _worker;
        private Timer _timer;

        private int _interval = 300;

        public TimedHostedService(IWorker worker, int interval)
        {
            _worker = worker;
            _interval = interval;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Random rnd = new Random();
            _interval += rnd.Next(-60, 60);
            if (_interval < 1) _interval = 1;   //Min interval 1 seg   
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_interval));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            await _worker.DoWork();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}