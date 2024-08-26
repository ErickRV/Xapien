using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Implementations
{
    public class DelayStep : IStep
    {
        private int milliseconds = 0;
        public DelayStep(int Milliseconds)
        {
            this.milliseconds = Milliseconds;
        }

        public async Task<StepResult> Run(ResultBag bag)
        {
            await Task.Delay(milliseconds);
            return null;
        }
    }
}
