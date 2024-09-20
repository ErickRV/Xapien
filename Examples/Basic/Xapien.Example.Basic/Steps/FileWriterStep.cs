using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Example.Basic.Steps
{
    internal class FileWriterStep : IStep
    {
        private readonly string fileName;
        private readonly string fileContent;

        public FileWriterStep(string fileName, string fileContent)
        {
            this.fileName = fileName;
            this.fileContent = fileContent;
        }

        public async Task<StepResult> Run(MemoryBag bag)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            await File.WriteAllTextAsync(fileName, fileContent);
            return new StepResult { ExitCode = 1, Output = null};
            
        }
    }
}
