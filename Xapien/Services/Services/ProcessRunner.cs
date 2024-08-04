using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;
using Xapien.Services.Interfaces;

namespace Xapien.Services.Services
{
    public class ProcessRunner : IProcessRunner
    {
        public Task<StepResult> RunProcess(string exePath, string parameters)
        {
            return Task.Run(() =>
            {

                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = Path.GetFullPath(exePath);
                startinfo.Arguments = parameters;
                Process process = new Process();
                process.StartInfo = startinfo;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();

                return new StepResult { ExitCode = process.ExitCode, Output = output };
            });
        }
    }
}
