using MechThrowverhaul.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace MechThrowverhaul.UI
{
    // This class represents the UIState for manufacturing table. It is similar to the Goblin Tinkerer's Reforge function. 
    internal class ManufacturingTableUI : UIState
	{
		public VanillaItemSlotWrapper vanillaItemSlot;
        public UIHoverImageButton RestockButton;

        const int slotX = 50;
        const int slotY = 270;
        public Player localPlayer;
        public ThrowingWeapons throwingWeapon;
        public int price;
        public int needed;

        public override void OnInitialize()
        {
			vanillaItemSlot = new VanillaItemSlotWrapper(ItemSlot.Context.PrefixItem, 0.85f)
            {
				Left = { Pixels = slotX },
				Top = { Pixels = slotY },
				ValidItemFunc = item => item.IsAir || (!item.IsAir && item.GetGlobalItem<ThrowingWeapons>().throwverhauled)
			};
			// Here we limit the items that can be placed in the slot. We are fine with placing an empty item in or a non-empty item that can be prefixed. Calling Prefix(-3) is the way to know if the item in question can take a prefix or not.
			Append(vanillaItemSlot);

            RestockButton = new UIHoverImageButton(ModContent.GetTexture("MechThrowverhaul/UI/RestockButton"), "Restock")
            {
                Left = { Pixels = slotX + 50},
                Top = { Pixels = slotY}
            };
            RestockButton.OnClick += OnButtonClickRestock;
        }

		// OnDeactivate is called when the UserInterface switches to a different state. In this mod, we switch between no state (null) and this state (ExamplePersonUI).
		// Using OnDeactivate is useful for clearing out Item slots and returning them to the player, as we do here.
		public override void OnDeactivate()
        {
			if (!vanillaItemSlot.Item.IsAir) {
				// QuickSpawnClonedItem will preserve mod data of the item. QuickSpawnItem will just spawn a fresh version of the item, losing the prefix.
				Main.LocalPlayer.QuickSpawnClonedItem(vanillaItemSlot.Item, vanillaItemSlot.Item.stack);
				// Now that we've spawned the item back onto the player, we reset the item by turning it into air.
				vanillaItemSlot.Item.TurnToAir();
			}
			// Note that in ExamplePerson we call .SetState(new UI.ExamplePersonUI());, thereby creating a new instance of this UIState each time. 
			// You could go with a different design, keeping around the same UIState instance if you wanted. This would preserve the UIState between opening and closing. Up to you.
		}
        
        // Update is called on a UIState while it is the active state of the UserInterface.
        // We use Update to handle automatically closing our UI when the player is no longer talking to our Example Person NPC.
        public override void Update(GameTime gameTime)
        {
			// Don't delete this or the UIElements attached to this UIState will cease to function.
			base.Update(gameTime);

            localPlayer = Main.LocalPlayer;
            if (Main.playerInventory == false || localPlayer.talkNPC != -1 || localPlayer.chest != -1 || localPlayer.sign != -1 || Vector2.Distance(localPlayer.position, localPlayer.GetModPlayer<ThrowPlayer>().ManufacturingTable) > 80)
            {
                localPlayer.GetModPlayer<ThrowPlayer>().UIOpen = false;
                ModContent.GetInstance<MechThrowverhaul>().ManufacturingTableUserInterface.SetState(null);
            }
        }

        private void OnButtonClickRestock(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.LocalPlayer.CanBuyItem(price, -1))
            {
                Main.LocalPlayer.BuyItem(price, -1);

                throwingWeapon.throwStack = vanillaItemSlot.Item.GetGlobalItem<ThrowingWeapons>().maxStack;

                Main.PlaySound(SoundID.Item37, -1, -1);
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
			base.DrawSelf(spriteBatch);

			// This will hide the crafting menu similar to the reforge menu. For best results this UI is placed before "Vanilla: Inventory" to prevent 1 frame of the craft menu showing.
			Main.HidePlayerCraftingMenu = true;

            if (!vanillaItemSlot.Item.IsAir)
            {
                throwingWeapon = vanillaItemSlot.Item.GetGlobalItem<ThrowingWeapons>();
                needed = throwingWeapon.maxStack - throwingWeapon.throwStack;
                price = needed * throwingWeapon.originalValue;
                if (price < 0)
                {
                    price = 0;
                }

                string costText = Language.GetTextValue("LegacyInterface.46") + ": ";
                string coinsText = "";
                int[] coins = Utils.CoinsSplit(price);
                if (coins[3] > 0)
                {
                    coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinPlatinum).Hex3() + ":" + coins[3] + " " + Language.GetTextValue("LegacyInterface.15") + "] ";
                }
                if (coins[2] > 0)
                {
                    coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinGold).Hex3() + ":" + coins[2] + " " + Language.GetTextValue("LegacyInterface.16") + "] ";
                }
                if (coins[1] > 0)
                {
                    coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinSilver).Hex3() + ":" + coins[1] + " " + Language.GetTextValue("LegacyInterface.17") + "] ";
                }
                coinsText = coinsText + "[c/" + Colors.AlphaDarken(Colors.CoinCopper).Hex3() + ":" + coins[0] + " " + Language.GetTextValue("LegacyInterface.18") + "] ";

                ItemSlot.DrawSavings(Main.spriteBatch, slotX + 130, Main.instance.invBottom, true);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, costText, new Vector2(slotX + 80, slotY + 10), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, coinsText, new Vector2(slotX + 80 + Main.fontMouseText.MeasureString(costText).X, (float)slotY + 10), Color.White, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                Append(RestockButton);
            }
            else
            {
                string message = "Place a throwing weapon here to restock";
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, message, new Vector2(slotX + 50, slotY), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
                
                RestockButton.Remove();
            }
        }
	}
}
