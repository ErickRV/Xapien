using System.CommandLine;
using System.CommandLine.Binding;

namespace Example.FileWriter
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            RootCommand rootCommand = new RootCommand("Xapien File Manager Example");
            rootCommand.AddCommand(CreateWriteFileCommand());
            rootCommand.AddCommand(CreateDeleteFileCommand());

            await rootCommand.InvokeAsync(args);
        }

        private static Command CreateWriteFileCommand() {
            Option pathOption = new Option<string>("-Path", "Ruta donde escribir el archivo");
            Option fileNameOption = new Option<string>("-Name", "Nombre del archivo a escribir");
            Option contentOption = new Option<string>("-Content", "Contenido a escribir en el archivo");

            Command writeCommand = new Command("WriteFile");
            writeCommand.AddOption(pathOption);
            writeCommand.AddOption(fileNameOption);
            writeCommand.AddOption(contentOption);

            writeCommand.SetHandler(ExecWriteFile,
                 (IValueDescriptor<string>)pathOption, (IValueDescriptor<string>)fileNameOption, (IValueDescriptor<string>)contentOption);

            return writeCommand;
        }

        private static void ExecWriteFile(string path, string fileName, string content)
        {
            string directoryPath = Path.GetFullPath(path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string completePath = directoryPath + fileName + ".txt";

            File.WriteAllText(completePath, content);
        }

        private static Command CreateDeleteFileCommand() {
            Option pathOption = new Option<string>("-Path", "Ruta del archivo a borrar");

            Command deleteFileCommand = new Command("KillFile");

            deleteFileCommand.AddOption(pathOption);
            deleteFileCommand.SetHandler(ExecKillFile, (IValueDescriptor<string>)pathOption);

            return deleteFileCommand;
        }

        private static void ExecKillFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
