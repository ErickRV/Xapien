using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

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
            Assert.IsTrue(xapienThread.Steps.Count() == 0);
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
    }
}

