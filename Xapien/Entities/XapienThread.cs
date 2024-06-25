using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Services.Interfaces;

namespace Xapien.Entities
{
    public class XapienThread
    {
        public IProcessRunner procRunner { get; private set; }

        public Task XTask { get; private set; }
        public string Name { get; private set; }
        public List<Step> Steps { get; private set; }

        public int currentStep { get; private set; }

        //TODO: Esto podria ser una clase diferente con la misma interface...
        // MultiThreaded = false
        // MaxThreadCount = 1


        public XapienThread(string name)
        {
            this.Name = name;
            this.currentStep = 0;
            Steps = new List<Step>();
        }

        //NOTE: List insertion order is guaranteed if no items are moved in the future 
        public void AddStep(Step step)
        {
            this.Steps.Add(step);
        }

        public void SetProcessRunner(IProcessRunner processRunner) {
            this.procRunner = processRunner;
        }

        public Task<StepResult> NextStep() {
            if (procRunner == default)
                throw new ArgumentNullException("ProcessRuner has not been set for this Thread!");

            Task<StepResult> result = Steps[currentStep].Run(this.procRunner);
            currentStep++;

            if (currentStep == Steps.Count)
                currentStep = 0;

            return result;
        }

        public Task InitThread(CancellationToken token) {
            XTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await NextStep();
                }
            });

            return XTask;
        }
    }
}
