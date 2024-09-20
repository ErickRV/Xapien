using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xapien.Entities;

namespace Xapien.Tests.Entities
{
    [TestClass]
    public class MemoryBagTests
    {
        [TestMethod]
        public void MemoryBag_Create()
        {
            //Arrange
            //Act
            MemoryBag memoryBag = new MemoryBag();

            //Assert
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = memoryBag.GetType();
            PropertyInfo field = type.GetProperty("memoryBag", flags);

            Assert.IsNotNull(field.GetValue(memoryBag));
        }

        [TestMethod]
        public void MemoryBag_SetItem_ItemDoesNotExists()
        {
            //Arrange
            MemoryBag memoryBag = new MemoryBag();

            //Act
            memoryBag.SetItem("Test", true);

            //Assert
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = memoryBag.GetType();
            PropertyInfo field = type.GetProperty("memoryBag", flags);
            Dictionary<string, object> internalMemory = (Dictionary<string, object>)field.GetValue(memoryBag);

            Assert.IsTrue(internalMemory.Any());
            
            bool storedValue = (bool)internalMemory.GetValueOrDefault("Test");
            Assert.AreEqual(storedValue, true);
        }

        [TestMethod]
        public void MemoryBag_SetItem_ItemAlreadyExists()
        {
            //Arrange 
            MemoryBag memoryBag = new MemoryBag();
            memoryBag.SetItem("Test", 1312);

            //Act
            memoryBag.SetItem("Test", "All Cats Are Beautiful");

            //Assert
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = memoryBag.GetType();
            PropertyInfo field = type.GetProperty("memoryBag", flags);
            Dictionary<string, object> internalMemory = (Dictionary<string, object>)field.GetValue(memoryBag);

            Assert.AreEqual(1, internalMemory.Count);

            string storedValue = (string)internalMemory.GetValueOrDefault("Test");
            Assert.AreEqual(storedValue, "All Cats Are Beautiful");
        }

        [TestMethod]
        public void GetItem_Item() {
            //Arrange 
            MemoryBag memoryBag = new MemoryBag();
            memoryBag.SetItem("Test", (byte)0XFF);

            //Act
            byte storedValue = (byte)memoryBag.GetItem("Test");

            //Assert
            Assert.AreEqual((byte)0XFF, storedValue);
        }

        [TestMethod]
        public void GetItem_Item_ItemDoesNotExists()
        {
            //Arrange 
            MemoryBag memoryBag = new MemoryBag();

            //Act
            var test = memoryBag.GetItem("Test");

            //Assert
            Assert.IsNull(test);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void ItemExists(bool exists)
        {
            //Arrange 
            MemoryBag memoryBag = new MemoryBag();

            if (exists)
                memoryBag.SetItem("Test", "Hello World!");

            //Act
            bool itemExists = memoryBag.ItemExists("Test");

            //Assert
            Assert.AreEqual(exists, itemExists);
        }

        [TestMethod]
        public void GetItemCount()
        {
            //Arrange 
            Random random = new Random();
            int itemCount = random.Next(1, 16);

            MemoryBag memoryBag = new MemoryBag();

            for (int i = 0; i < itemCount; i++)
            {
                memoryBag.SetItem(Guid.NewGuid().ToString(), random.Next());
            }

            //Act
            int count = memoryBag.Count;

            //Assert
            Assert.AreEqual(itemCount, count);
        }
    }
}
