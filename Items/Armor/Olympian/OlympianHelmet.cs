using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Armor.Olympian
{
    [AutoloadEquip(EquipType.Head)]
    public class OlympianHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("7% incerased throwing damage" + "\n10% Increased throwing critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += 0.07f;
            player.thrownCrit += 10;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<OlympianChestplate>() && legs.type == ModContent.ItemType<OlympianLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Thrown weapons gain 6 armor penetration";
            if (player.inventory[player.selectedItem].thrown == true)
            {
                player.armorPenetration += 6;
            }
        }
    }
}