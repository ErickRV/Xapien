using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Example.Basic.Steps
{
    internal class FileKillerStep : IStep
    {
        private readonly string filename;

        public FileKillerStep(string filename)
        {
            this.filename = filename;
        }

        public async Task<StepResult> Run(ResultBag bag)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            return new StepResult { ExitCode = 1 };
        }
    }
}
