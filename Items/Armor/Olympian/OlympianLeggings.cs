using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MechThrowverhaul.Items.Armor.Olympian
{
    [AutoloadEquip(EquipType.Legs)]
    public class OlympianLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            Tooltip.SetDefault("7% incerased throwing damage" + "\n5% Increased throwing critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 10000;
            item.rare = ItemRarityID.Green;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownDamage += 0.07f;
            player.thrownCrit += 5;
        }
    }
}