using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xapien.Core;
using Xapien.Entities;
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
            Assert.IsTrue(builder.ProcessRunner != null);
            ProcessRunner testCast = (ProcessRunner)builder.ProcessRunner;
        }

        public void AddXapienThread_Ok() {  
            //Arrange 
            XapienBuilder builder = new XapienBuilder();

            string threadName = Faker.Random.String2(8);
            List<Step> steps = new List<Step> {
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep(),
                MockDataGenerator.CreateMockStep()
            };
            //Act

            //Assert
        }
    }
}
