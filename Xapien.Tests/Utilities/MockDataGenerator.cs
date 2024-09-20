using AutoBogus;
using Bogus;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;
using Xapien.Services.Interfaces;

namespace Xapien.Tests.Utilities
{
    internal class MockDataGenerator
    {
        public static IStep CreateMockStep() {
            Mock<IStep> mock = new Mock<IStep>();
            mock.Setup(x => x.Run(It.IsAny<MemoryBag>()));
            return mock.Object;
        }

        public static IStep CreateMockStep(out Mock<IStep> mocker)
        {
            Mock<IStep> mock = new Mock<IStep>();
            mock.Setup(x => x.Run(It.IsAny<MemoryBag>()));

            mocker = mock;
            return mock.Object;
        }

        public static MockStep CreateMockStepImplementation() {
            return new MockStep();
        }

        public static StepResult CreateMockStepResult() {
            return AutoFaker.Generate<StepResult>();
        }

        public static XapienThread CreateMockXapienThread(IStep step, int StepCount){
            Faker faker = new Faker();
            XapienThread xapienThread = new XapienThread(faker.Random.String2(8));
            for (int i = 0; i < StepCount; i++) {
                xapienThread.AddStep(step);
            }

            return xapienThread;
        }
    }

    internal class MockStep : IStep
    {
        public Task<StepResult> Run(MemoryBag bag)
        {
            return Task.FromResult(new AutoFaker<StepResult>().Generate());
        }
    }
}
