using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Core
{
    public class Orchestrator
    {
        private CancellationToken token;

        private Task CoreThread;
        private List<XapienThread> Threads;

        public Task Start(CancellationToken token) {
            return new Task(() => {
                while (true) {
                    if (token.IsCancellationRequested)
                        return;

                    //TODO: Ciclo infinito de procesamiento
                }
            });
        }
    }
}
