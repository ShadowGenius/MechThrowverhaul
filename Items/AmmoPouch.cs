using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items
{
    public class AmmoPouch : ModItem
    {
        // This item was used for testing purposes

        public override string Texture => "Terraria/Item_" + ItemID.EndlessMusketPouch;

        public override void SetDefaults()
        {
            item.thrown = true;
            item.width = 26;
            item.height = 34;
            base.SetDefaults();
        }

        // can be picked up despite having no space
        public override bool ItemSpace(Player player) => true;

        // calls the Restock method in ThrowPlayer to restock their weapons
        public override bool OnPickup(Player player)
        {
            player.GetModPlayer<ThrowPlayer>().Restock(item.stack, 0);
            
            Main.PlaySound(SoundID.Grab);
            return false;
        }
    }
}