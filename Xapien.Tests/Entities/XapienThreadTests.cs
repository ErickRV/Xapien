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
        }

        [TestMethod]
        public void SetProcessRunner_Ok() {
            //Arrange 
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            Mock<IProcessRunner> mockProcRunner = new Mock<IProcessRunner>();
            IProcessRunner processRunner = mockProcRunner.Object;

            //Act
            xapienThread.SetProcessRunner(processRunner);

            //Assert
            Assert.AreSame(processRunner, xapienThread.procRunner);

        }

        [TestMethod]
        public void AddStep() {
            //Arrange
            XapienThread xapienThread = new XapienThread("INSERTS");

            string Route = faker.Random.String(8);
            string Command = faker.Random.String(8);
            string Args = faker.Random.String(8);

            Step step1 = new Step(Route, Command, Args);

            //Act
            xapienThread.AddStep(step1);

            //Assert
            Assert.IsTrue(xapienThread.Steps.Count == 1);
            Assert.AreEqual(xapienThread.Steps.First().Route, step1.Route);
            Assert.AreEqual(xapienThread.Steps.First().Command, step1.Command);
            Assert.AreEqual(xapienThread.Steps.First().Args, step1.Args);
        }

        [TestMethod]
        public async Task NextStep_FirstStep_Ok() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            Mock<IProcessRunner> mockProcRunner = new Mock<IProcessRunner>();
            StepResult stepResult = MockDataGenerator.CreateMockStepResult();

            mockProcRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(stepResult));
            xapienThread.SetProcessRunner(mockProcRunner.Object);

            Step step0 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step0);

            Step step1 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step1);

            //Act
            StepResult result = await xapienThread.NextStep();

            //Assert
            Assert.AreEqual(1, xapienThread.currentStep);
            Assert.IsTrue(TestUtils.AreObjectsEqual(stepResult, result));

            mockProcRunner.Verify(x => x.RunProcess(step0.Route, $"{step0.Command} {step0.Args}"), Times.Once);
            mockProcRunner.Verify(x => x.RunProcess(step1.Route, $"{step1.Command} {step1.Args}"), Times.Never);
        }

        [TestMethod]
        public async Task NextStep_SecondStep_Ok() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            Mock<IProcessRunner> mockProcRunner = new Mock<IProcessRunner>();
            StepResult step1Result = MockDataGenerator.CreateMockStepResult();

            Step step0 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step0);

            Step step1 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step1);

            mockProcRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(MockDataGenerator.CreateMockStepResult()));

            mockProcRunner.Setup(x => x.RunProcess(step1.Route, $"{step1.Command} {step1.Args}"))
                .Returns(Task.FromResult(step1Result));

            xapienThread.SetProcessRunner(mockProcRunner.Object);

            //Act
            await xapienThread.NextStep();
            StepResult result = await xapienThread.NextStep();

            //Assert
            Assert.AreEqual(0, xapienThread.currentStep);
            Assert.IsTrue(TestUtils.AreObjectsEqual(step1Result, result));

            mockProcRunner.Verify(x => x.RunProcess(step0.Route, $"{step0.Command} {step0.Args}"), Times.Once);
            mockProcRunner.Verify(x => x.RunProcess(step1.Route, $"{step1.Command} {step1.Args}"), Times.Once);
        }

        [TestMethod]
        public async Task NextStep_NewCycle() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            Mock<IProcessRunner> mockProcRunner = new Mock<IProcessRunner>();

            Step step0 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step0);

            Step step1 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step1);

            mockProcRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(MockDataGenerator.CreateMockStepResult()));

            xapienThread.SetProcessRunner(mockProcRunner.Object);

            //Act
            await xapienThread.NextStep();
            await xapienThread.NextStep();
            await xapienThread.NextStep();

            //Assert
            Assert.AreEqual(1, xapienThread.currentStep);
            mockProcRunner.Verify(x => x.RunProcess(step0.Route, $"{step0.Command} {step0.Args}"), Times.Exactly(2));
            mockProcRunner.Verify(x => x.RunProcess(step1.Route, $"{step1.Command} {step1.Args}"), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task NextStep_Error_NoProcRunner() {
            //Arrange
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));

            Step step0 = MockDataGenerator.CreateMockStep();
            xapienThread.AddStep(step0);

            //Act
            await xapienThread.NextStep();
        }
    }
}

