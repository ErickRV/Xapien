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
        private bool state = false;

        public async Task<StepResult> Run(ResultBag bag)
        {
            if (!Directory.Exists("Output"))
                Directory.CreateDirectory("Output");

            if (!state)
            {
                if (File.Exists("Output/HolaMundo.txt"))
                    File.Delete("Output/HolaMundo.txt");
            }
            else {
                await File.WriteAllTextAsync("Output/HolaMundo.txt", "Hola Mundo!");
            }

            state = !state;
            await Task.Delay(500);

            return null;
        }
    }
}
