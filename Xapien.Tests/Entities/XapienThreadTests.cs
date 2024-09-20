using Bogus;
using Bogus.DataSets;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;
using Xapien.Services.Interfaces;
using Xapien.Tests.Utilities;

namespace Xapien.Tests.Entities
{
    [TestClass]
    public class XapienThreadTests
    {
        Faker faker = new Faker();

        class ExceptionStep : IStep
        {
            public Task<StepResult> Run(MemoryBag bag)
            {
                return null;
            }
        }

        [TestMethod]
        public void CreateXapienThread() {
            //Arrange 
            string name = faker.Random.String(8);

            //Act
            XapienThread xapienThread = new XapienThread(name);

            //Assert
            Assert.AreEqual(name, xapienThread.Name);
            Assert.AreEqual(0, xapienThread.currentStep);
            Assert.IsTrue(xapienThread.Steps.Count() == 0);
            Assert.IsNotNull(xapienThread.MemoryBag);
        }

        [TestMethod]
        public void AddStep() {
            //Arrange
            XapienThread xapienThread = new XapienThread("INSERTS");

            string Route = faker.Random.String(8);
            string Command = faker.Random.String(8);
            string Args = faker.Random.String(8);

            IStep step1 = MockDataGenerator.CreateMockStep();

            //Act
            xapienThread.AddStep(step1);

            //Assert
            Assert.IsTrue(xapienThread.Steps.Count == 1);
            Assert.AreEqual(xapienThread.Steps.First(), step1);
        }

        [TestMethod]
        public async Task NextStep_FirstStep_Ok() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            StepResult stepResult = MockDataGenerator.CreateMockStepResult();

            IStep step0 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker0);
            mocker0.Setup(r => r.Run(It.IsAny<MemoryBag>()))
                .Returns(Task.FromResult(stepResult));
            
            xapienThread.AddStep(step0);

            IStep step1 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker1);
            xapienThread.AddStep(step1);

            //Act
            StepResult result = await xapienThread.NextStep();

            //Assert
            Assert.AreEqual(1, xapienThread.currentStep);
            Assert.IsTrue(TestUtils.AreObjectsEqual(stepResult, result));

            mocker0.Verify(x => x.Run(It.IsAny<MemoryBag>()), Times.Once);
            mocker1.Verify(x => x.Run(It.IsAny<MemoryBag>()), Times.Never);
        }

        [TestMethod]
        public async Task NextStep_SecondStep_Ok() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            StepResult step1Result = MockDataGenerator.CreateMockStepResult();

            IStep step0 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker0);
            xapienThread.AddStep(step0);

            IStep step1 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker1);
            xapienThread.AddStep(step1);

            mocker0.Setup(x => x.Run(It.IsAny<MemoryBag>()))
                .Returns(Task.FromResult(MockDataGenerator.CreateMockStepResult()));

            mocker1.Setup(x => x.Run(It.IsAny<MemoryBag>()))
                .Returns(Task.FromResult(step1Result));

            //Act
            await xapienThread.NextStep();
            StepResult result = await xapienThread.NextStep();

            //Assert
            Assert.AreEqual(0, xapienThread.currentStep);
            Assert.IsTrue(TestUtils.AreObjectsEqual(step1Result, result));

            mocker0.Verify(x => x.Run(It.IsAny<MemoryBag>()), Times.Once);
            mocker0.Verify(x => x.Run(It.IsAny<MemoryBag>()), Times.Once);
        }

        [TestMethod]
        public async Task NextStep_NewCycle() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            IStep step0 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker0);
            xapienThread.AddStep(step0);

            IStep step1 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker1);
            xapienThread.AddStep(step1);

            //Act
            await xapienThread.NextStep();
            await xapienThread.NextStep();
            await xapienThread.NextStep();

            //Assert
            Assert.AreEqual(1, xapienThread.currentStep);
            mocker0.Verify(x => x.Run(It.IsAny<MemoryBag>()), Times.Exactly(2));
            mocker1.Verify(x => x.Run(It.IsAny<MemoryBag>()), Times.Once);
        }

        [TestMethod]
        public async Task InitThread_Ok() {
            //Arrange
            int stepsToRun = 5;
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            int stepCounter = 0;

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            IStep step = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker);
            xapienThread.AddStep(step);
            xapienThread.AddStep(step);
            xapienThread.AddStep(step);

            mocker.Setup(r => r.Run(It.IsAny<MemoryBag>()))
                .Returns(Task.Run(async () => 
                {
                    await Task.Delay(10);
                    return MockDataGenerator.CreateMockStepResult();
                }))
                .Callback(() => { 
                    stepCounter++;
                    if (stepCounter == stepsToRun)
                        source.Cancel();
                });

            //Act
            int mainThreadCounter = 0;
            Task xTask = xapienThread.InitThread(token);
            while (!xTask.IsCompleted) {
                mainThreadCounter++;
            }

            //Assert
            Assert.IsTrue(mainThreadCounter > 0);
            mocker.Verify(r => r.Run(It.IsAny<MemoryBag>()), Times.Exactly(stepsToRun));

        }

        [TestMethod]
        public async Task InitThread_ExceptionIsThrown()
        {
            //Arrange 
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            IStep step0 = MockDataGenerator.CreateMockStep(out Mock<IStep> mocker);
            mocker.Setup(m => m.Run(It.IsAny<MemoryBag>()))
                .Throws(new Exception());

            xapienThread.AddStep(step0);

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            //Act 
            int mainThreadCounter = 0;
            Task xTask = xapienThread.InitThread(token);
            while (!xTask.IsCompleted){
                mainThreadCounter++;
            }

            //Assert
            Assert.IsTrue(xTask.Status == TaskStatus.Faulted);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task NexStep_ReturnsNull()
        {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(15));
            xapienThread.AddStep(new ExceptionStep());

            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            //Act
            await xapienThread.InitThread(token);
        }
    }
}

