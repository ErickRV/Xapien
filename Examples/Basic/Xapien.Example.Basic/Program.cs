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
            builder.AddXThread("Hilo 1", 
                new List<Step> 
                { 
                    new Step("C:\\Xapien Show\\Programs\\FileManager\\Example.FileWriter.exe",
                    "WriteFile -Path \"C:\\Xapien Show\\XThread\\\\\" -Name \"Hola\"",
                    ""),

                    new Step("C:\\Xapien Show\\Programs\\FileManager\\Example.FileWriter.exe",
                    "WriteFile -Path \"C:\\Xapien Show\\XThread\\\\\" -Name \"Mundo\"",
                    ""),

                    new Step("C:\\Xapien Show\\Programs\\FileManager\\Example.FileWriter.exe",
                    "KillFile -Path \"C:\\Xapien Show\\XThread\\Hola.txt\"",
                    ""),

                    new Step("C:\\Xapien Show\\Programs\\FileManager\\Example.FileWriter.exe",
                    "KillFile -Path \"C:\\Xapien Show\\XThread\\Mundo.txt\"",
                    ""),
                });

            Xapien.Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
    }
}
