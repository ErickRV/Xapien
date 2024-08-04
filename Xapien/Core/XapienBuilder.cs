using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Services.Interfaces;
using Xapien.Services.Services;

namespace Xapien.Core
{
    public class XapienBuilder
    {
        public IProcessRunner ProcessRunner { get; private set; }

        public XapienBuilder()
        {
            this.ProcessRunner = new ProcessRunner();
        }
    }
}
