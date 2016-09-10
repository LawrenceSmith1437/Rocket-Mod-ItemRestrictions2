using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.Unturned.Items;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using Rocket.API;
using UnityEngine;
//using UnityEngine.Events;
using Logger = Rocket.Core.Logging.Logger;

namespace ItemRestrictions2
{
    public class PluginIR : RocketPlugin<IR_Config>
    {
        public static PluginIR Instance;
        public string Version = "2.0.0.1";

        private const string PermissionIgnoreRetrictions = "ItemRestriction.IgnoreRestrictions";

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {
                    { "item_notPermitted", "Item not permitted: {0}" },
                    { "translation_version_dont_edit", "1" }
                };
            }
        }

        protected override void Load()
        {
            Instance = this;
            UnturnedPlayerEvents.OnPlayerInventoryAdded += IR_ItemAdded;

            Logger.LogWarning("Setting up Item Restrictions by SgtSpamCan with Original by LeeIzaZombie. v" + Version);
            Logger.LogWarning("--");
            Logger.LogWarning(string.Format("Black listed items found: {0}", Configuration.Instance.RestrictedItems.Count));
            Logger.LogWarning(string.Format("IgnoreAdmin = {0}", Configuration.Instance.IgnoreAdmin));
            Logger.LogWarning("Item Restrictions is ready!");
            Logger.LogWarning("--");

            foreach (Item item in Configuration.Instance.RestrictedItems)
            {
                WriteTraceMessage(string.Format("ItemId = {0}", item.ID));
            }
        }

        private void IR_ItemAdded(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar p)
        {
            WriteTraceMessage("in event handler");

            WriteTraceMessage(string.Format("Player {0}, IsAdmin = {1}", player.DisplayName, player.IsAdmin));

            if (player.IsAdmin && Configuration.Instance.IgnoreAdmin)
                return;

            var permissions = player.GetPermissions();

            foreach (var permission in permissions)
            {
                if (permission.Name == PermissionIgnoreRetrictions)
                {
                    WriteTraceMessage(string.Format("Player {0}, HasPermission {1}",
                        player.DisplayName,
                        PermissionIgnoreRetrictions));
                    return;
                }
            }

            foreach (Item item in Configuration.Instance.RestrictedItems)
            {
                if (item.ID == p.Item.ItemID)
                {
                    WriteTraceMessage(string.Format("Player just received forbidden item {0}", item.ID));

                    while (true)
                    {
                        /*  If we remove an item, we may blow up if we keep rummaging because the 
                         *  items in the pages may shift position/count in their data structures.
                         *  best to rescan until no more contraband is found.
                         */
                        if (!RemoveContraband(player, p.item.ItemID))
                            break;                    
                    }
                    UnturnedChat.Say(
                        player,
                        Translate("item_notPermitted", UnturnedItems.GetItemAssetById(p.item.ItemID).name),
                        Color.red
                        );
                }
            }
        }

        private bool RemoveContraband(UnturnedPlayer player, ushort contrabandItemId)// ItemJar p)
        {
            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                WriteTraceMessage(string.Format("-> page {0}", page));


                byte itemCount = 0;
                bool hasPage = false;
                try
                {
                    itemCount = player.Inventory.getItemCount(page);
                    hasPage = true;
                }
                catch 
                {
                    //skip this page if no items present or page not present.
                    WriteTraceMessage("this page not present");
                }

                if (hasPage)
                {
                    WriteTraceMessage(string.Format("---> item count {0}", itemCount));

                    for (byte index = 0; index < itemCount; index++)
                    {
                        WriteTraceMessage(string.Format("-----> index {0}, item {1}", index,
                            player.Inventory.getItem(page, index).Item.ItemID));
                        if (player.Player.inventory.getItem(page, index).Item.ItemID == contrabandItemId)
                        {
                            WriteTraceMessage("Removing Item");
                            player.Inventory.removeItem(page, index);
                            WriteTraceMessage("Item Removed");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void WriteTraceMessage(string message)
        {
#if DEBUG
            Logger.Log(message);
#endif
        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerInventoryAdded -= IR_ItemAdded;
        }
    }
}
