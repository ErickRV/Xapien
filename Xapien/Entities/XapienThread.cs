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
        public Task XTask { get; private set; }
        public string Name { get; private set; }
        public List<IStep> Steps { get; private set; }
        public ResultBag ResultBag { get; private set; }

        public int currentStep { get; private set; }

        //TODO: Esto podria ser una clase diferente con la misma interface...
        // MultiThreaded = false
        // MaxThreadCount = 1
        //Considerar poder ingresar una XapienThread como Step...


        public XapienThread(string name)
        {
            this.Name = name;
            this.currentStep = 0;
            Steps = new List<IStep>();
        }

        //NOTE: List insertion order is guaranteed if no items are moved in the future 
        public void AddStep(IStep step)
        {
            this.Steps.Add(step);
        }

        public Task<StepResult> NextStep() {
            Task<StepResult> result = Steps[currentStep].Run(ResultBag);
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
