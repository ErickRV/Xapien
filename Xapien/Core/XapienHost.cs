using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xapien.Core
{
    public class XapienHost : IHostedService
    {
        private Task? _xapienTask;
        private CancellationTokenSource? _stoppingCts;
        private readonly Xapien xapien;

        public XapienHost(Xapien xapien)
        {
            this.xapien = xapien;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Create linked token to allow cancelling executing task from provided token
            _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            xapien.SetCancellationTokenSource(_stoppingCts);

            _xapienTask = xapien.Run();

            // If the task is completed then return it, this will bubble cancellation and failure to the caller
            if (_xapienTask.IsCompleted)
            {
                return _xapienTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_xapienTask == null)
            {
                return;
            }

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts!.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                var tcs = new TaskCompletionSource<object>();
                using CancellationTokenRegistration registration = cancellationToken.Register(s => ((TaskCompletionSource<object>)s!).SetCanceled(), tcs);
                // Do not await the _executeTask because cancelling it will throw an OperationCanceledException which we are explicitly ignoring
                await Task.WhenAny(_xapienTask, tcs.Task).ConfigureAwait(false);
            }
        }
    }
}
