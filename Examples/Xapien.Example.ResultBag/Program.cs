using Xapien.Core;
using Xapien.Entities;
using Xapien.Example.ResultBag.Steps;

namespace Xapien.Example.ResultBag
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            XapienBuilder builder = new XapienBuilder();

            RandomNumberStep randomNumber = new RandomNumberStep();
            PrintStep printStep = new PrintStep();

            builder.AddXThread("Hilo 1", new List<IStep> { 
                randomNumber,
                printStep
            });

            Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
    }
}
