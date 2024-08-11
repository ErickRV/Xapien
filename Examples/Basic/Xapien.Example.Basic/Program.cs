using Xapien.Core;
using Xapien.Entities;
using Xapien.Example.Basic.Steps;

namespace Xapien.Example.Basic
{
    internal class Program
    {
        //This is the Orchestrator class
        static async Task Main(string[] args)
        {
            XapienBuilder builder = new XapienBuilder();

            builder.AddXThread("X Thread", new List<IStep> {
                new FileWriterStep("Output\\XThread\\Hello.txt", "Hello"),
                new FileWriterStep("Output\\XThread\\World.txt", "World"),
                new FileKillerStep("Output\\XThread\\Hello.txt"),
                new FileKillerStep("Output\\XThread\\World.txt")
            });

            builder.AddXThread("Y Thread", new List<IStep> {
                new FileWriterStep("Output\\YThread\\Hello.txt", "Hello"),
                new FileWriterStep("Output\\YThread\\World.txt", "World"),
                new FileKillerStep("Output\\YThread\\Hello.txt"),
                new FileKillerStep("Output\\YThread\\World.txt")
            });

            Xapien.Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
    }
}
