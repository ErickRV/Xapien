using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Example.ResultBag.Steps
{
    public class PrintStep : IStep
    {
        public async Task<StepResult> Run(Entities.ResultBag bag)
        {
            StepResult previousResult = bag.SetpResults["random"];
            Console.WriteLine($"Previous Step generated random number: {(int)previousResult.Output}");

            await Task.Delay(500);
            
            return null; //To return null this method MUST BE DECLARED async
        }
    }
}
