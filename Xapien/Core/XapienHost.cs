using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xapien.Core
{
    public class XapienHost : BackgroundService
    {
        private Task? _xapienTask;
        private CancellationTokenSource? _stoppingCts;
        private readonly Xapien xapien;

        public XapienHost(Xapien xapien)
        {
            this.xapien = xapien;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                // Create linked token to allow cancelling executing task from provided token
                _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

                xapien.SetCancellationTokenSource(_stoppingCts);

                _xapienTask = xapien.Run();

                await _xapienTask;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
