using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Armor.Olympian
{
    [AutoloadEquip(EquipType.Body)]
    public class OlympianChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("7% incerased throwing damage" + "\n20% Increased throwing velocity");
        }

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 26;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += 0.07f;
            player.thrownVelocity += 0.20f;
        }
    }
}