using MechThrowverhaul.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace MechThrowverhaul
{
    public class ThrowPlayer : ModPlayer
    {
        public int storedStock;
        public Vector2 ManufacturingTable;
        public bool UIOpen;

        public int Restock(int stock, int type)
        {
            for (int i = 0; i < Main.maxInventory; i++) // loops through the player's inventory
            {
                Item currentItem = player.inventory[i];
                // determines if the item:
                if (currentItem.active && currentItem.type != ItemID.None && (currentItem.type == type || type == 0)) // Exists and matches the item type or is universal,
                {
                    ThrowingWeapons thrownItem = currentItem.GetGlobalItem<ThrowingWeapons>();

                    if (thrownItem.throwverhauled == true && // has been throwverhauled,
                        thrownItem.throwStack < thrownItem.maxStack) // and hasn't already met the max ammo count
                    {
                        thrownItem.throwStack += stock; // increases the current stack by the value of "stock"
                        stock -= stock; // reduces the stock to 0 because it should be empty
                        if (thrownItem.throwStack > thrownItem.maxStack) // if the ammo count is increased above the maximum
                        {
                            stock += thrownItem.throwStack - thrownItem.maxStack; // stock is increased by the remainder / the extra change
                                                                                  // this extra change is used to restock more throwing weapons in the inventory
                            thrownItem.throwStack = thrownItem.maxStack; // set ammo count back to max
                        }
                        if (stock <= 0) break; // if there is no stock left, no point in continuing
                    }
                }
            }
            return stock;
        }

        public override TagCompound Save()
        {
            return new TagCompound
            {
                {"storedStock", storedStock}
            };
        }

        public override void Load(TagCompound tag)
        {
            storedStock = tag.GetInt("storedStock");
        }
    }
}