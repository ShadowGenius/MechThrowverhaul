using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Boomerangs
{
    public abstract class ThrownBoomerang : ModItem
    {
        public override void SetDefaults()
        {
            item.thrown = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.consumable = true;
            item.maxStack = 5;
            item.GetGlobalItem<ThrowingWeapons>().boomerang = true;
            item.GetGlobalItem<ThrowingWeapons>().maxStack = 5;
            base.SetDefaults();
        }
    }

    public abstract class ThrownBoomerangProj : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.thrown = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.aiStyle = 3;
            projectile.penetrate = -1;
            base.SetDefaults();
        }

        public override void AI()
        {
            for (int i = 0; i < Main.maxItems; i++)
            {
                Item item = Main.item[i];
                if (item.type != ItemID.None && item.GetGlobalItem<ThrowingWeapons>().droppedAmmo && projectile.Hitbox.Intersects(item.Hitbox))
                {
                    item.Center = projectile.Center;
                }
            }
            base.AI();
        }
    }
}