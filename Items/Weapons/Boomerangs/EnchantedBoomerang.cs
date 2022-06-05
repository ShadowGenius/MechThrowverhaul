using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Boomerangs
{
    public class EnchantedBoomerang : ThrownBoomerang
    {
        public override string Texture => "Terraria/Item_" + ItemID.EnchantedBoomerang;

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.EnchantedBoomerang);
            item.shoot = mod.ProjectileType("EnchantedBoomerangProj");
            base.SetDefaults();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.EnchantedBoomerang);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }

    public class EnchantedBoomerangProj : ThrownBoomerangProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.EnchantedBoomerang;

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.EnchantedBoomerang);
            aiType = ProjectileID.EnchantedBoomerang;
            base.SetDefaults();
        }
    }
}