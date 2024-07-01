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

            MainThread = Task.Run(() => {
                CancellationToken token = CancellationTokenSource.Token;
                foreach (XapienThread xapienThread in threads) {
                    xapienThread.InitThread(token);
                }

                while (!token.IsCancellationRequested) {
                    //TODO: How do we want Xapien to behave when any thread is faulted???
                    //For the moment it will cancel all other tasks and die...
                    if (threads.Select(t => t.XTask).Any(x => x.Status == TaskStatus.Faulted)) {
                        CancellationTokenSource.Cancel();
                    }
                }
            });

            return MainThread;
        }
    }
}
