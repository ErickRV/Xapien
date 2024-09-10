using Xapien.Core;
using Xapien.Entities;
using Xapien.Example.State.Steps;

namespace Xapien.Example.State
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IStep state = new StateStep();

            XapienBuilder builder = new XapienBuilder();
            builder.AddXThread("Hilo X", new List<Entities.IStep> { 
                state
            });

            Xapien.Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
    }
}
