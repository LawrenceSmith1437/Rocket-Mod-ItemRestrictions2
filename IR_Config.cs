using Rocket.API;
using Rocket.Core.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ItemRestrictions2
{
    public class IR_Config : IRocketPluginConfiguration
    {
        [XmlArrayItem(ElementName = "Item")]
        public List<Item> RestrictedItems;

        public bool IgnoreAdmin;

        public void LoadDefaults()
        {
            this.IgnoreAdmin = true;
            this.RestrictedItems = new List<Item>
            {
                new Item(519, "Rocket Launcher"),
                new Item(1100,"Sticky Grenade" ),
                new Item(1240,"Detonator"),
                new Item(1353,"Stealy Wheely Automobiley")
            };
        }
    }
}
