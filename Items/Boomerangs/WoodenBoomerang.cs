using Terraria;
using Terraria.ID;

namespace MechThrowverhaul.Items.Boomerangs
{
    public class WoodenBoomerang : ThrownBoomerang
    {
        public override string Texture => "Terraria/Item_" + ItemID.WoodenBoomerang;

        public override void SetDefaults()
        {
            item.damage = 8;
            item.knockBack = 5;
            item.shootSpeed = 6.5f;
            item.value = Item.buyPrice(0, 0, 50, 0);
            item.useAnimation = item.useTime = 16;
            item.width = 14;
            item.height = 28;
            item.shoot = mod.ProjectileType("WoodenBoomerangProj");
            base.SetDefaults();
        }
    }

    public class WoodenBoomerangProj : ThrownBoomerangProj
    {
        public override string Texture => "Terraria/Projectile_" + ProjectileID.WoodenBoomerang;

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 22;
            base.SetDefaults();
        }
    }
}