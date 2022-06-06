using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Boomerangs
{
    public class Flamarang : ThrownBoomerang
    {
        public override string Texture => "Terraria/Item_" + ItemID.Flamarang;

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Flamarang);
            item.shoot = mod.ProjectileType("FlamarangProj");
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Flamarang);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }

    public class FlamarangProj : ThrownBoomerangProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.Flamarang;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Flamarang);
            aiType = ProjectileID.Flamarang;
            base.SetDefaults();
        }
    }
}