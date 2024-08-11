using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Core;
using Xapien.Entities;
using Xapien.Services.Interfaces;
using Xapien.Services.Services;
using Xapien.Tests.Utilities;

namespace Xapien.Tests.Core
{
    [TestClass]
    public class XapienBuilderTests
    {
        Faker Faker = new Faker();

        [TestMethod]
        public void CreateXapienBuilder_Ok() {
            //Arrange 

            //Act
            XapienBuilder builder = new XapienBuilder();

            //Assert
            Assert.AreEqual(0, builder.XThreads.Count);
        }

        [TestMethod]
        public void AddXapienThread_Ok() {  
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string threadName = Faker.Random.String2(8);
            List<IStep> steps = new List<IStep> {
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation(),
            };

            //Act
            builder.AddXThread(threadName, steps);

            //Assert
            Assert.IsTrue(builder.XThreads.Any());
            XapienThread thread = builder.XThreads.First(x => x.Name == threadName);

            List<MockStep> mockSteps = steps.Select(x => (MockStep)x).ToList();
            List<MockStep> threadMockSteps = thread.Steps.Select(x => (MockStep)x).ToList();
            Assert.IsTrue(TestUtils.AreObjectsEqual(threadMockSteps, mockSteps));

            int randomIndex = Faker.Random.Int(0, steps.Count - 1);
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps[randomIndex], thread.Steps[randomIndex]));
        }

        [TestMethod]
        public void AddXapienThread_AddMultiple() {
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string thread1Name = Faker.Random.String2(8);
            List<IStep> steps1 = new List<IStep> {
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation()
            };

            string thread2Name = Faker.Random.String2(8);
            List<IStep> steps2 = new List<IStep> {
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation()
            };

            //Act
            builder.AddXThread(thread1Name, steps1);
            builder.AddXThread(thread2Name, steps2);

            //Assert
            Assert.AreEqual(2, builder.XThreads.Count);

            Assert.IsTrue(TestUtils.AreObjectsEqual(thread1Name, builder.XThreads[0].Name));
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps1.Select(x => (MockStep)x).ToList(), builder.XThreads[0].Steps.Select(x => (MockStep)x).ToList()));

            Assert.IsTrue(TestUtils.AreObjectsEqual(thread2Name, builder.XThreads[1].Name));
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps2.Select(x => (MockStep)x).ToList(), builder.XThreads[1].Steps.Select(x => (MockStep)x).ToList()));
        }

        [TestMethod]
        public void Build_Ok() {
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string threadName = Faker.Random.String2(8);
            List<IStep> steps = new List<IStep> {
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation()
            };

            string thread2Name = Faker.Random.String2(8);
            List<IStep> steps2 = new List<IStep> {
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation(),
                MockDataGenerator.CreateMockStepImplementation()
            };

            builder.AddXThread(threadName, steps);
            builder.AddXThread(thread2Name, steps2);

            //Act 
            Xapien.Core.Xapien xapien = builder.Build();

            //Assert
            XapienThread x1 = xapien.threads.First(x => x.Name == threadName);
            XapienThread x2 = xapien.threads.First(x => x.Name == thread2Name);

            Assert.IsTrue(TestUtils.AreObjectsEqual(steps.Select(x => (MockStep)x).ToList(), x1.Steps.Select(x => (MockStep)x).ToList()));
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps2.Select(x => (MockStep)x).ToList(), x2.Steps.Select(x => (MockStep)x).ToList()));
        }
    }
}
