using Xapien.Core;
using Xapien.Entities;
using Xapien.Example.Basic.Steps;
using Xapien.Implementations;

namespace Xapien.Example.Basic
{
    internal class Program
    {
        //This is the Orchestrator class
        static async Task Main(string[] args)
        {
            XapienBuilder builder = new XapienBuilder();
            DelayStep delay = new DelayStep(500);

            builder.AddXThread("X Thread", new List<IStep> {
                new FileWriterStep("Output\\XThread\\Hello.txt", "Hello"),
                delay,
                new FileWriterStep("Output\\XThread\\World.txt", "World"),
                delay,
                new FileKillerStep("Output\\XThread\\Hello.txt"),
                delay,
                new FileKillerStep("Output\\XThread\\World.txt"),
                delay
            });

            builder.AddXThread("Y Thread", new List<IStep> {
                new FileWriterStep("Output\\YThread\\Hello.txt", "Hello"),
                delay,
                new FileWriterStep("Output\\YThread\\World.txt", "World"),
                delay,
                new FileKillerStep("Output\\YThread\\Hello.txt"),
                delay,
                new FileKillerStep("Output\\YThread\\World.txt"),
                delay,
            });

            Xapien.Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
    }
}
