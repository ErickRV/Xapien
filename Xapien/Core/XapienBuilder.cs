using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;
using Xapien.Services.Interfaces;
using Xapien.Services.Services;

namespace Xapien.Core
{
    public class XapienBuilder
    {
        public List<XapienThread> XThreads { get; private set; }

        public XapienBuilder()
        {
            this.XThreads = new List<XapienThread>();
        }

        public void AddXThread(string threadName, List<IStep> steps)
        {
            XapienThread xapienThread = new XapienThread(threadName);
            for (int i = 0; i < steps.Count; i++)
                xapienThread.Steps.Add(steps[i]);

            XThreads.Add(xapienThread);
        }

        public Xapien Build()
        {
            return new Xapien(this.XThreads);
        }
    }
}
