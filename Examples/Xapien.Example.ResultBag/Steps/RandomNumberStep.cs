using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Example.ResultBag.Steps
{
    public class RandomNumberStep : IStep
    {
        public async Task<StepResult> Run(MemoryBag bag)
        {
            int random = new Random().Next();
            if (bag.ItemExists("random")){
                bag.SetItem("random", new StepResult { Output = random });
            }
            else {
                bag.SetItem("random", new StepResult { Output = random });
            }

            return null; //To return null this method MUST BE DECLARED async
        }
    }
}
