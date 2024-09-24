# X A P I E N  

### About
Xapien is a data processing micro framework  that aims to simplify the initial setup of a data processing application and allow developers to simply focus on the processing logic.   
<br/>
With Xapien developers can easily create an infinite loop with multiple threads that run sequentially "logic packages" that have state and can share information between each other  

<br/><br/>
## Quick Start

#### Install nuguet package
```
PM> NuGet\Install-Package Xapien
```

#### Write an IStep implementation
```
    public class PrintStep : IStep
    {
        public async Task<StepResult> Run(Entities.ResultBag bag)
        {
            Console.WriteLine($"Hello World again and again...");

            await Task.Delay(500);
            
            return null; //To return null this method MUST BE DECLARED async
        }
    }
```

#### Create a Builder, add your IStep, build and run!
```
        static async Task Main(string[] args)
        {
            XapienBuilder builder = new XapienBuilder();
            PrintStep printStep = new PrintStep();

            builder.AddXThread("Thread One", new List<IStep> { 
                printStep
            });

            Core.Xapien xapien = builder.Build();
            await xapien.Run();
        }
```

<br/><br/>
## Learn more

You can find more examples [here](https://github.com/ErickRV/Xapien/tree/main/Examples)

