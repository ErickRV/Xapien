using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xapien.Entities
{
    /*This is just a wrapper class that abstracts the functionalities of a dictionary
     in a way easy to use in xapien*/
    public class MemoryBag
    {
        private Dictionary<string, object> memoryBag { get; set; }
        public int Count { get { return memoryBag.Count; } }

        public MemoryBag()
        {
            memoryBag = new Dictionary<string, object>();
        }

        public void SetItem(string itemName, object itemValue)
        {
            if(memoryBag.ContainsKey(itemName))
                memoryBag[itemName] = itemValue;
            else
                memoryBag.Add(itemName, itemValue);
        }

        public object GetItem(string itemName)
        {
            if (memoryBag.ContainsKey(itemName))
                return memoryBag[itemName];
            else
                return null;
        }

        public bool ItemExists(string itemName)
        {
            return memoryBag.ContainsKey(itemName);
        }

    }
}
