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
        class CustomProcRunner : IProcessRunner
        {
            public Task<StepResult> RunProcess(string exePath, string parameters)
            {
                return Task.FromResult(new StepResult());
            }
        }

        [TestMethod]
        public void CreateXapienBuilder_Ok() {
            //Arrange 

            //Act
            XapienBuilder builder = new XapienBuilder();

            //Assert
            Assert.IsTrue(builder.ProcessRunner != null);
            Assert.AreEqual(0, builder.XThreads.Count);
            ProcessRunner testCast = (ProcessRunner)builder.ProcessRunner;
        }

        [TestMethod]
        public void AddXapienThread_Ok() {  
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string threadName = Faker.Random.String2(8);
            List<Step> steps = new List<Step> {
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep()
            };

            //Act
            builder.AddXThread(threadName, steps);

            //Assert
            Assert.IsTrue(builder.XThreads.Any());

            XapienThread thread = builder.XThreads.First(x => x.Name == threadName);

            Assert.AreEqual(builder.ProcessRunner, thread.procRunner);

            Assert.IsTrue(TestUtils.AreObjectsEqual(thread.Steps, steps));

            int randomIndex = Faker.Random.Int(0, steps.Count - 1);
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps[randomIndex], thread.Steps[randomIndex]));
        }

        [TestMethod]
        public void AddXapienThread_AddMultiple() {
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string thread1Name = Faker.Random.String2(8);
            List<Step> steps1 = new List<Step> {
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep()
            };

            string thread2Name = Faker.Random.String2(8);
            List<Step> steps2 = new List<Step> {
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep()
            };

            //Act
            builder.AddXThread(thread1Name, steps1);
            builder.AddXThread(thread2Name, steps2);

            //Assert
            Assert.AreEqual(2, builder.XThreads.Count);

            Assert.IsTrue(TestUtils.AreObjectsEqual(thread1Name, builder.XThreads[0].Name));
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps1, builder.XThreads[0].Steps));
            Assert.AreEqual(builder.ProcessRunner, builder.XThreads[0].procRunner);

            Assert.IsTrue(TestUtils.AreObjectsEqual(thread2Name, builder.XThreads[1].Name));
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps2, builder.XThreads[1].Steps));
            Assert.AreEqual(builder.ProcessRunner, builder.XThreads[1].procRunner);
        }

        [TestMethod]
        public void SetCustomProcRunner() {
            //Arrange 
            XapienBuilder builder = new XapienBuilder();
            IProcessRunner processRunner = new CustomProcRunner();

            //Act
            builder.SetProcessRunner(processRunner);

            //Assert
            CustomProcRunner testCast = (CustomProcRunner)builder.ProcessRunner;
        }

        [TestMethod]
        public void Build_Ok() {
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string threadName = Faker.Random.String2(8);
            List<Step> steps = new List<Step> {
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep()
            };

            string thread2Name = Faker.Random.String2(8);
            List<Step> steps2 = new List<Step> {
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep()
            };

            builder.AddXThread(threadName, steps);
            builder.AddXThread(thread2Name, steps2);

            //Act 
            Xapien.Core.Xapien xapien = builder.Build();

            //Assert
            XapienThread x1 = xapien.threads.First(x => x.Name == threadName);
            XapienThread x2 = xapien.threads.First(x => x.Name == thread2Name);

            Assert.IsTrue(TestUtils.AreObjectsEqual(steps, x1.Steps));
            Assert.IsTrue(TestUtils.AreObjectsEqual(steps2, x2.Steps));
        }
    }
}
