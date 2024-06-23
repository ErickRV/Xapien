using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Services.Interfaces;

namespace Xapien.Entities
{
    public class Step
    {
        public string Route { get; private set; }
        public string Command { get; private set; }
        public string Args { get; private set; }

        public Step(string Route, string Command, string Args = null)
        {
            this.Route = Route;
            this.Command = Command;
            this.Args = Args;
        }

        public Task<StepResult> Run(IProcessRunner procRunner)
        {
            return procRunner.RunProcess(this.Route, $"{this.Command} {this.Args}");
        }
    }
}
