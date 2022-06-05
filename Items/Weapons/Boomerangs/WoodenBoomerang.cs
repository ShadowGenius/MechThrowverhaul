using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Boomerangs
{
    public class WoodenBoomerang : ThrownBoomerang
    {
        public override string Texture => "Terraria/Item_" + ItemID.WoodenBoomerang;

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodenBoomerang);
            item.shoot = mod.ProjectileType("WoodenBoomerangProj");
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodenBoomerang);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }

    public class WoodenBoomerangProj : ThrownBoomerangProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.WoodenBoomerang;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
            base.SetDefaults();
        }
    }
}