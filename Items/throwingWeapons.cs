using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace MechThrowverhaul.Items
{
    public class ThrowingWeapons : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        public bool throwverhauled;
        public bool boomerang;
        public bool drawStack = false;
        public int throwStack = 1;
        public int maxStack = 100;
        public int originalValue;
        public bool droppedAmmo;
        public int timer;

        public override void SetDefaults(Item item)
        {
            // Bones are a special case where they are both materials and weapons, so I've set them to ranged damage for now
            // Bone gloves and ale tossers are much closer to ranged than thrown
            if (item.type == ItemID.Bone || item.type == ItemID.BoneGlove || item.type == ItemID.AleThrowingGlove)
            {
                item.thrown = false;
                item.ranged = true;
            }

            // sets up the conversion into the "throwverhauled" system
            // They aren't fully converted here in SetDefaults in case of already existing items such as those generated in chests
            if (item.thrown == true && item.consumable == true)
            {
                item.consumable = false;
                item.maxStack = maxStack;
                throwverhauled = true;
                if (boomerang)
                {
                    originalValue = item.value / maxStack;
                }
                else
                {
                    originalValue = item.value;
                    item.value = originalValue * maxStack;
                }
            }
            base.SetDefaults(item);
        }

        public override void UpdateInventory(Item item, Player player)
        {
            if (throwverhauled == true)
            {
                if (item.stack > 1) // this is for thrown weapons that spawn naturally with stacks higher than 1
                {
                    // for example, if you find 69 shurikens in a chest, it will be converted into 69 "ammo"
                    throwStack = item.stack;
                    item.stack = 1;
                }
                // makes sure ammo doesn't go beyond the max amount
                if (throwStack > maxStack)
                {
                    throwStack = maxStack;
                }
                // fully converts items into the "throwverhauled" system
                item.maxStack = 1;
                drawStack = true;
            }
            base.UpdateInventory(item, player);
        }

        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            // draws the text for the ammo count 
            // only does this when it updates in the inventory in case of items with stack sizes more than one
            // this code is modified and adapted from spirit mod code for glyph icons
            if (drawStack == true)
            {
                Vector2 SlotDimensions = new Vector2(52, 52); // Item slots are 52 pixels wide and 52 pixels tall

                float slotScale = 1f;

                if (frame.Width > 32 || frame.Height > 32)
                {
                    if (frame.Width > frame.Height)
                        slotScale = 32f / frame.Width;
                    else
                        slotScale = 32f / frame.Height;
                }

                slotScale *= Main.inventoryScale; // Scale of the slot changes when you select it in the hotbar
                Vector2 slotOrigin = position + frame.Size() * (.5f * slotScale); // Position is the center, so the top-left of the slot (or origin) is the center + half the width and height, then adjust for the slot scale
                slotOrigin -= SlotDimensions * (.5f * Main.inventoryScale); // 

                Vector2 offset = new Vector2(0, SlotDimensions.Y); // Offset the string to the bottom of the item slot
                Vector2 stringSize = Main.fontItemStack.MeasureString(throwStack.ToString()) * 0.75f * Main.inventoryScale; // Measures how large the string would be
                offset.X += 8;
                offset.Y -= stringSize.Y + 4; // Adjust the Y position depending on the size of the string
                offset *= Main.inventoryScale; // Adjust for the slot scale

                Utils.DrawBorderString(spriteBatch, throwStack.ToString(), slotOrigin + offset, Color.White, 0.75f * Main.inventoryScale);
            }
            base.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override bool CanUseItem(Item item, Player player)
        {
            // prevents using the weapon when there is 0 ammo left
            if (throwverhauled == true)
            {
                if (throwStack == 0)
                {
                    return false;
                }
            }
            return base.CanUseItem(item, player);
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            // If the player has a chance not to consume thrown weapons, apply it here. Consumes 1 ammo from the weapon otherwise
            bool consume = true;
            if ((player.thrownCost50 && Main.rand.Next(100) < 50) || (player.thrownCost33 && Main.rand.Next(100) < 33))
            {
                consume = false;
            }
            if (consume)
            {
                throwStack--;
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        // This functionality will be moved to an accessory later down the line
        //public override void HoldItem(Item item, Player player)
        //{
        //    //creates a dotted line "prediction" for the projectile when a "throwverhauled" weapon is held

        //   timer++;
        //    if (throwverhauled && Main.myPlayer == player.whoAmI && timer == 15) //creates predictions once every 15/60 or 1/4 second
        //    {
        //        // vectors are fun
        //        Vector2 velocity = Main.MouseWorld - player.Center; // gives us the vector between the player's mouse and the player themselves
        //        velocity.Normalize(); // finds a unit vector, which tells us the direction of the projectile
        //        velocity *= item.shootSpeed; // mutiplies the unit vector, or direction, by the projectile's speed, which gives us velocity

        //        int projectile = Projectile.NewProjectile(player.Center, velocity, item.shoot, 0, 0, player.whoAmI);
        //        Main.projectile[projectile].GetGlobalProjectile<ThrowingProjectiles>().prediction = true;
        //        timer = 0;
        //    }
        //}

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            // Allows throwverhauled weapons to gain universal modifiers + ranged modifiers
            // Eventually, the ranged modifiers will be replaced by throwing-specific modifiers
            if (throwverhauled)
            {
                return rand.Next(new int[] { PrefixID.Keen, PrefixID.Superior, PrefixID.Forceful, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy, PrefixID.Hurtful, PrefixID.Strong, PrefixID.Unpleasant, PrefixID.Weak, PrefixID.Ruthless, PrefixID.Godly, PrefixID.Demonic, PrefixID.Zealous,
                PrefixID.Quick, PrefixID.Deadly, PrefixID.Agile, PrefixID.Nimble, PrefixID.Murderous, PrefixID.Slow, PrefixID.Sluggish, PrefixID.Lazy, PrefixID.Annoying, PrefixID.Nasty,
                PrefixID.Sighted, PrefixID.Rapid, PrefixID.Hasty, PrefixID.Intimidating, PrefixID.Deadly, PrefixID.Staunch, PrefixID.Awful, PrefixID.Lethargic, PrefixID.Awkward, PrefixID.Powerful, PrefixID.Frenzying, PrefixID.Unreal});
            }
            return base.ChoosePrefix(item, rand);
        }

        // Dropped ammo items can be picked up despite having no space
        public override bool ItemSpace(Item item, Player player)
        {
            if (droppedAmmo)
            {
                return true;
            }
            return base.ItemSpace(item, player);
        }

        // calls the Restock method in ThrowPlayer to restock their weapons
        // also shows the text and makes the sound of picking up an item if it goes through
        public override bool OnPickup(Item item, Player player)
        {
            if (droppedAmmo)
            {
                int leftover = player.GetModPlayer<ThrowPlayer>().Restock(item.stack, item.type);
                if (leftover == 0)
                {
                    ItemText.NewText(item, item.stack);
                    Main.PlaySound(SoundID.Grab);
                }
                return false;
            }
            return base.OnPickup(item, player);
        }

        // saves the value for the ammo count; else the value will be reset every time you load
        public override bool NeedsSaving(Item item)
        {
            if (throwverhauled)
            {
                return true;
            }
            return base.NeedsSaving(item);
        }

        public override TagCompound Save(Item item)
        {
            return new TagCompound
            {
                {"throwStack", throwStack}
            };
        }

        public override void Load(Item item, TagCompound tag)
        {
            throwStack = tag.GetInt("throwStack");
        }
    }
}