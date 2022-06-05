using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Boomerangs
{
    public class IceBoomerang : ThrownBoomerang
    {
        public override string Texture => "Terraria/Item_" + ItemID.IceBoomerang;

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.IceBoomerang);
            item.shoot = mod.ProjectileType("IceBoomerangProj");
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceBoomerang);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }

    public class IceBoomerangProj : ThrownBoomerangProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.IceBoomerang;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.IceBoomerang);
            aiType = ProjectileID.IceBoomerang;
            base.SetDefaults();
        }
    }
}