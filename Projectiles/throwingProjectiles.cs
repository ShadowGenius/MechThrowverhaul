using MechThrowverhaul.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Projectiles
{
    public class ThrowingProjectiles : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool CloneNewInstances => true;

        public bool throwverhauled;
        public int itemType;
        public bool prediction;
        public float distance = 0;

        public override void SetDefaults(Projectile projectile)
        {
            // sets the javelin and bone javelin to throwing damage because for some reason they were melee in vanilla??
            if (projectile.type == ProjectileID.BoneJavelin || projectile.type == ProjectileID.JavelinFriendly)
            {
                projectile.thrown = true;
                projectile.melee = false;
            }
            for (int i = 0; i < ItemLoader.ItemCount; i++) // Loops through all loaded items, both vanilla and modded
            {
                // Allows us to access each item's values in SetDefaults
                Item item = new Item();
                item.SetDefaults(i); 
                // If the item is a throwverhauled weapon and the projectile it shoots matches this projectile, throwverhaul the projectile
                if (item.GetGlobalItem<ThrowingWeapons>().throwverhauled == true && item.shoot == projectile.type)
                {
                    throwverhauled = true;
                    itemType = item.type;
                    projectile.noDropItem = true;
                }
            }
            base.SetDefaults(projectile);
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.thrown && !prediction)
            {
                distance += Vector2.Distance(projectile.oldPosition, projectile.position); // Keeps track of the distance the projectile has traveled
            }

            if (prediction) // if the projectile is a prediction projectile, it will:
            {
                projectile.alpha = 255; // become invisible,
                projectile.friendly = projectile.hostile = false; // not deal damage to enemies or players,
                projectile.extraUpdates = 100; // travel almost instantly

                projectile.localAI[1] += 1f;
                if (projectile.localAI[1] == 10f) // every ten ticks, 4 dusts will be spawned; This creates the dotted line pattern
                {
                    // this code is adapted from the shadowbeam staff projectile code
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 projectilePosition = projectile.position;
                        projectilePosition -= projectile.velocity * i * 0.25f;
                        int dust = Dust.NewDust(projectilePosition, 1, 1, DustID.PortalBolt, 0f, 0f, 0, default, 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = projectilePosition;
                        Main.dust[dust].scale = Main.rand.Next(70, 110) * 0.013f;
                        Main.dust[dust].velocity *= 0.2f;
                    }

                    projectile.localAI[1] = 0;
                }
            }
            base.AI(projectile);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            // Adds 1 damage to thrown projectiles for every 10 blocks traveled
            // Experimental feature
            //if (projectile.thrown)
            //{
            //    damage += (int)(distance / 16 / 10); 
            //    // Divides by 16 because a block is 16 pixels
            //    // Divides by 10 so 1 damage is added for every 10 blocks
            //}
            base.ModifyHitNPC(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            if (throwverhauled && Main.myPlayer == projectile.owner)
            {
                // Throwverhauled projectiles drop their corresponding items as ammo instead of actual items
                int item = Item.NewItem(projectile.getRect(), itemType, 1);
                Main.item[item].GetGlobalItem<ThrowingWeapons>().droppedAmmo = true;
                //return false;
            }
            return !prediction; // if the projectile is a prediction projectile, vanilla and some modded "kill" code won't be called
        }

        public override void Kill(Projectile projectile, int timeLeft)
        {
            //if (throwverhauled && Main.myPlayer == projectile.owner)
            //{
            //    // Throwverhauled projectiles drop their corresponding items as ammo instead of actual items
            //    int item = Item.NewItem(projectile.getRect(), itemType, 1);
            //    Main.item[item].GetGlobalItem<ThrowingWeapons>().droppedAmmo = true;
            //}
        }
    }
}