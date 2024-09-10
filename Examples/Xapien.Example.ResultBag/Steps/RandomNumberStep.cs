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
        public async Task<StepResult> Run(Entities.ResultBag bag)
        {
            int random = new Random().Next();
            if (bag.SetpResults.ContainsKey("random")){
                bag.SetpResults["random"] = new StepResult { Output = random };
            }
            else {
                bag.SetpResults.Add("random", new StepResult { Output = random });
            }

            return null; //To return null this method MUST BE DECLARED async
        }
    }
}
