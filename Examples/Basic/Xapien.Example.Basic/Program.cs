using Xapien.Core;
using Xapien.Entities;

namespace Xapien.Example.Basic
{
    internal class Program
    {
        //This is the Orchestrator class
        static async Task Main(string[] args)
        {
            
            XapienBuilder builder = new XapienBuilder();
            Xapien.Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
    }
}
