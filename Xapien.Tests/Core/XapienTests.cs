using Bogus;
using Moq;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Core;
using Xapien.Entities;
using Xapien.Services.Interfaces;
using Xapien.Tests.Utilities;

namespace Xapien.Tests.Core
{
    [TestClass]
    public class XapienTests
    {
        Faker _faker = new Faker();
        
        [TestMethod]
        public void CreateXapien_Ok() {
            //Arrange 
            Mock<IProcessRunner> mockProcessRunner = new Mock<IProcessRunner>();

            List<XapienThread> threads = new List<XapienThread>()
            {
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
            };

            //Act
            Xapien.Core.Xapien xapien = new Xapien.Core.Xapien(threads);

            //Assert
            Assert.AreEqual(threads, xapien.threads);
        }

        [TestMethod]
        public void SetCancellationTokenSource_Ok() {
            //Arrange 
            Mock<IProcessRunner> mockProcessRunner = new Mock<IProcessRunner>();

            List<XapienThread> threads = new List<XapienThread>()
            {
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
            };
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            //Act
            Xapien.Core.Xapien xapien = new Xapien.Core.Xapien(threads);
            xapien.SetCancellationTokenSource(tokenSource);

            //Assert
            Assert.AreEqual(tokenSource, xapien.CancellationTokenSource);
        }

        [TestMethod]
        public async Task Run_NoCancellationSourceSet_Ok() {
            //Arrange 
            Mock<IProcessRunner> mockProcessRunner = new Mock<IProcessRunner>();
            int processRunerExecCounter = 0;
            mockProcessRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(async () => {
                    await Task.Delay(50);
                    return MockDataGenerator.CreateMockStepResult();
                }))
                .Callback(() => { 
                    processRunerExecCounter++;
                });

            List<XapienThread> threads = new List<XapienThread>()
            {
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
            };

            //Act
            Xapien.Core.Xapien xapien = new Xapien.Core.Xapien(threads);
            Task mainThread = xapien.Run();
            await Task.Delay(500); //Give time to all threads to init...

            //Assert
            Assert.IsNotNull(xapien.CancellationTokenSource);
            int randomSteps = _faker.Random.Int(5, 20);
            while (processRunerExecCounter < randomSteps) {
                Assert.AreEqual(TaskStatus.Running, mainThread.Status);
                Assert.IsTrue(xapien.threads.Select(t => t.XTask).All(x => x.Status == TaskStatus.Running));
            }

            xapien.CancellationTokenSource.Cancel();
        }

        [TestMethod]
        public async Task Run_CancellationSourceSet_Ok() {
            //Arrange 
            Mock<IProcessRunner> mockProcessRunner = new Mock<IProcessRunner>();
            int processRunerExecCounter = 0;
            mockProcessRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Run(async () => {
                    await Task.Delay(50);
                    return MockDataGenerator.CreateMockStepResult();
                }))
                .Callback(() => {
                    processRunerExecCounter++;
                });

            List<XapienThread> threads = new List<XapienThread>()
            {
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
            };

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            Xapien.Core.Xapien xapien = new Xapien.Core.Xapien(threads);
            xapien.SetCancellationTokenSource(tokenSource);

            //Act
            Task mainThread = xapien.Run();
            await Task.Delay(500); //Give time to all threads to init...

            //Assert
            Assert.AreEqual(tokenSource, xapien.CancellationTokenSource);

            int randomSteps = _faker.Random.Int(5, 20);
            while (processRunerExecCounter < randomSteps)
            {
                Assert.AreEqual(TaskStatus.Running, mainThread.Status);
                Assert.IsTrue(xapien.threads.Select(t => t.XTask).All(x => x.Status == TaskStatus.Running));
            }

            tokenSource.Cancel();
        }

        [TestMethod]
        public async Task Run_ThreadIsFaulted() {
            //Arrange 
            int randomSteps = _faker.Random.Int(5, 20);

            Mock<IProcessRunner> mockProcessRunner = new Mock<IProcessRunner>();

            mockProcessRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());

            List<XapienThread> threads = new List<XapienThread>()
            {
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
                MockDataGenerator.CreateMockXapienThread(mockProcessRunner.Object, _faker.Random.Int(1, 10)),
            };

            //Act
            Xapien.Core.Xapien xapien = new Xapien.Core.Xapien(threads);
            Task mainThread = xapien.Run();
            await Task.Delay(500); //Give time to all threads to init...

            int counter = 0;
            while (!mainThread.IsCompleted) { 
                await Task.Delay(10);
                counter++;

                if (counter == 1312) {
                    xapien.CancellationTokenSource.Cancel();
                    await Task.Delay(500);
                    Assert.IsTrue(false); // This test failed because thread did not stop 
                }
            }
            
            //Assert
            Assert.IsTrue(xapien.threads.Select(x => x.XTask)
                .All(x => x.Status == TaskStatus.Canceled || x.Status == TaskStatus.Faulted));
        }
    }
}
