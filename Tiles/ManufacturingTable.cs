using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MechThrowverhaul.Tiles
{
    public class ManufacturingTable : ModTile
    {
        public bool OpenUI;

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            //TileObjectData.newTile.CoordinateHeights = new[] { 18 };
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Manufacturing Table");
            AddMapEntry(new Color(200, 200, 200), name);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<Items.Placeables.ManufacturingTable>());
        }

        public override bool NewRightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;
            int left = i * 16;
            int top = j * 16;

            if (player.GetModPlayer<ThrowPlayer>().UIOpen == true)
            {
                ModContent.GetInstance<MechThrowverhaul>().ManufacturingTableUserInterface.SetState(null);
                player.GetModPlayer<ThrowPlayer>().UIOpen = false;
            }
            else
            {
                player.GetModPlayer<ThrowPlayer>().ManufacturingTable = new Vector2(left, top);
                Main.playerInventory = true;
                if (player.sign >= 0 || player.chest >= 0 || player.talkNPC >= 0)
                {
                    Main.PlaySound(SoundID.MenuClose);
                    player.sign = -1;
                    player.chest = -1;
                    player.talkNPC = -1;
                    player.flyingPigChest = -1;
                    Main.editSign = false;
                    Main.npcChatText = "";
                }
                ModContent.GetInstance<MechThrowverhaul>().ManufacturingTableUserInterface.SetState(new UI.ManufacturingTableUI());
                player.GetModPlayer<ThrowPlayer>().UIOpen = true;
            }
            
            return true;
        }
    }
}