using Bogus;
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
    public class StepTests
    {
        Faker faker = new Faker();

        [TestMethod]
        public void CreateStep() {
            //Arrange 
            string Route = faker.Random.String(8);
            string Command = faker.Random.String(8);
            string Args = faker.Random.String(8);

            //Act
            Step Step = new Step(Route, Command, Args);

            //Assert
            Assert.AreEqual(Route, Step.Route);
            Assert.AreEqual(Command, Step.Command);
            Assert.AreEqual(Args, Step.Args);
        }

        [TestMethod]
        public void CreateStep_NoArgs() {
            //Arrange 
            string Route = faker.Random.String(8);
            string Command = faker.Random.String(8);

            //Act
            Step Step = new Step(Route, Command);

            //Assert
            Assert.AreEqual(Route, Step.Route);
            Assert.AreEqual(Command, Step.Command);
            Assert.AreEqual(null, Step.Args);
        }

        [TestMethod]
        public async Task StepRun_Ok() {
            //Arrange 
            Step step = MockDataGenerator.CreateMockStep();
            StepResult stepResult = new StepResult()
            {
                ExitCode = 0,
                Output = string.Empty
            };

            Mock<IProcessRunner> mockProcessRunner = new Mock<IProcessRunner>();
            mockProcessRunner.Setup(x => x.RunProcess(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(stepResult));

            //Act
            StepResult result = await step.Run(mockProcessRunner.Object);

            //Assert
            Assert.IsTrue(TestUtils.AreObjectsEqual(stepResult, result));

            mockProcessRunner.Verify(x => x.RunProcess(step.Route, $"{step.Command} {step.Args}"), Times.Once);
        }
    }
}
