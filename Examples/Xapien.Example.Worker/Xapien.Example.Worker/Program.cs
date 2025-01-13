using Xapien.Core;
using Xapien.Entities;
using Xapien.Example.Worker.Steps;

namespace Xapien.Example.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RandomNumberStep randomNumber = new RandomNumberStep();
            PrintStep printStep = new PrintStep();

            XapienBuilder xapienBuilder = new XapienBuilder();
            xapienBuilder.AddXThread("Hilo 1", new List<IStep> {
                randomNumber,
                printStep
            });

            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSingleton(xapienBuilder.Build());
            builder.Services.AddHostedService<XapienHost>();

            var host = builder.Build();
            host.Run();
        }
    }
}