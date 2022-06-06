using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Boomerangs
{
    public class ThornChakram : ThrownBoomerang
    {
        public override string Texture => "Terraria/Item_" + ItemID.ThornChakram;

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.ThornChakram);
            item.shoot = mod.ProjectileType("ThornChakramProj");
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ThornChakram);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }

    public class ThornChakramProj : ThrownBoomerangProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.ThornChakram;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ThornChakram);
            aiType = ProjectileID.ThornChakram;
            base.SetDefaults();
        }
    }
}