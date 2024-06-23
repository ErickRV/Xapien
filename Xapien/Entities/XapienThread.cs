using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xapien.Entities
{
    public class XapienThread
    {
        public Task XTask { get; private set; }
        public string Name { get; private set; }
        public List<Step> Steps { get; private set; } 

        //TODO:
        // MultiThreaded = false
        // MaxThreadCount = 1


        public XapienThread(string name)
        {
            this.Name = name;
            Steps = new List<Step>();
        }

        //NOTE: List insertion order is guaranteed if no items are moved in the future 
        public void AddStep(Step step)
        {
            this.Steps.Add(step);
        }

        //TODO:
        //public Task InitThread(CancellationToken token) {
        //    XTask = Task.Run(async () => {
        //        while (!token.IsCancellationRequested) {
        //            await NextStep();
        //        }
        //    });
        //    return XTask;
        //}
    }
}
