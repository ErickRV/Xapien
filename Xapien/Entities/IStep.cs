using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Services.Interfaces;

namespace Xapien.Entities
{
    public interface IStep
    {
        public Task<StepResult> Run(MemoryBag bag);
    }
}
