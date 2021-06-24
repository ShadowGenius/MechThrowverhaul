using MechThrowverhaul.Items;
using MechThrowverhaul.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.ID;

namespace MechThrowverhaul
{
    public class MechThrowverhaul : Mod
	{
        internal ManufacturingTableUI ManufacturingTableUI;

        internal UserInterface ManufacturingTableUserInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {

                ManufacturingTableUI = new ManufacturingTableUI();
                ManufacturingTableUI.Activate();
                ManufacturingTableUserInterface = new UserInterface();
                ManufacturingTableUserInterface.SetState(ManufacturingTableUI);
                ModContent.GetInstance<MechThrowverhaul>().ManufacturingTableUserInterface.SetState(null);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            ManufacturingTableUserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
            if (InventoryIndex != -1)
            {
                layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
                    "MechThrowverhaul: ManufacturingTable",
                    delegate
                    {
                        ManufacturingTableUserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        // Experimental system to automatically adjust recipes for throwverhauled weapons

        public override void PostAddRecipes()
        {
            RecipeFinder finder = new RecipeFinder(); // make a new RecipeFinder

            foreach (Recipe recipe in finder.SearchRecipes()) // loop every recipe found by the finder
            {
                ThrowingWeapons throwItem = recipe.createItem.GetGlobalItem<ThrowingWeapons>();
                if (throwItem.throwverhauled)
                {
                    RecipeEditor editor = new RecipeEditor(recipe); // for the currently looped recipe, make a new RecipeEditor
                    
                    double multiplicand = throwItem.maxStack / recipe.createItem.stack;
                    if (multiplicand <= 0) multiplicand = 1;

                    recipe.createItem.stack = throwItem.maxStack;

                    foreach (Item item in recipe.requiredItem)
                    {
                        item.stack = (int)Math.Round(item.stack * multiplicand);
                    }
                }

                foreach (Item item in recipe.requiredItem)
                {
                    if (item.type != ItemID.None && item.GetGlobalItem<ThrowingWeapons>().throwverhauled)
                    {
                        item.stack = 1;
                    }
                }
            }
        }
    }
}