using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Services.Interfaces
{
    public interface IProcessRunner
    {
        Task<StepResult> RunProcess(string exePath, string parameters);
    }
}
