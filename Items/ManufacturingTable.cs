using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items
{
    public class ManufacturingTable : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows you to restock throwing weapons.");
        }

        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 41;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 50, 0);
            item.createTile = ModContent.TileType<Tiles.ManufacturingTable>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IronBar, 10);
            recipe.AddIngredient(ItemID.Wood, 15);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.LeadBar, 10);
            recipe2.AddIngredient(ItemID.Wood, 15);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}