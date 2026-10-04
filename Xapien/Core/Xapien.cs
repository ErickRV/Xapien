using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;
using Xapien.Services.Interfaces;

namespace Xapien.Core
{
    public class Xapien
    {
        public List<XapienThread> threads { get; private set; }
        public Task MainThread { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; } = null;

        public Xapien(List<XapienThread> Xthreads)
        {
            this.threads = Xthreads;
        }

        public void SetCancellationTokenSource(CancellationTokenSource tokenSource)
        {
            this.CancellationTokenSource = tokenSource;
        }

        public Task Run()
        {
            if (CancellationTokenSource == default)
                CancellationTokenSource = new CancellationTokenSource();

            MainThread = Task.Run(async () => 
            {
                try
                {
                    CancellationToken token = CancellationTokenSource.Token;
                    List<Task> tasks = new List<Task>();
                    foreach (XapienThread xapienThread in threads)
                    {
                        Task task = xapienThread.InitThread(token);
                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks);
                }
                catch (Exception)
                {
                    throw;
                }
            });

            return MainThread;
        }
    }
}
