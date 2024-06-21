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
    }
}
