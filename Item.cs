using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemRestrictions2
{
    public class Item
    {
        public Item() { }
        public ushort ID { get; set; }

        public Item(ushort id, string descr)
        {
            ID = id;
            Descr = descr;
        }

        public string Descr { get; set; }
    }
}
