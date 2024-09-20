using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Implementations
{
    public class RunProcStep : IStep
    {
        private string Name;
        private string exePath;
        private string parameters;

        public RunProcStep(string Name, string ExePath, string Parameters)
        {
            this.Name = Name;
            exePath = ExePath;
            parameters = Parameters;
        }

        public async Task<StepResult> Run(MemoryBag bag)
        {
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = Path.GetFullPath(exePath);
            startinfo.Arguments = parameters;
            Process process = new Process();
            process.StartInfo = startinfo;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            await process.WaitForExitAsync();
            string output = process.StandardOutput.ReadToEnd();

            StepResult result = new StepResult { ExitCode = process.ExitCode, Output = output };
            bag.SetItem(Name, result);

            return result;
        }
    }
}
