using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Example.State.Steps
{
    public class StateStep : IStep
    {

        public async Task<StepResult> Run(ResultBag bag)
        {
            if (!Directory.Exists("Output"))
                Directory.CreateDirectory("Output");

            if (bag.SetpResults.Any(x => x.Key == "Hola"))
            {
                if (File.Exists("Output/HolaMundo.txt"))
                    File.Delete("Output/HolaMundo.txt");

                bag.SetpResults.Remove("Hola");
            }
            else {
                await File.WriteAllTextAsync("Output/HolaMundo.txt", "Hola");
                bag.SetpResults.Add("Hola", null);
            }

            await Task.Delay(500);
            return new StepResult { ExitCode = 1 };
        }
    }
}
