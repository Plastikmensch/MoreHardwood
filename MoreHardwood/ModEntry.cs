using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;

namespace MoreHardwood
{
    public class ModEntry : Mod
    {
        /// <summary>The config.</summary>
        private static ModConfig Config;

        /// <summary>The mod helper.</summary>
        private static IModHelper ModHelper;

        /// <summary>The mod monitor.</summary>
        private static IMonitor ModMonitor;

        /// <summary>Reference to a tree</summary>
        private static Tree tree;

        /// <summary>Array of valid items.</summary>
        private static int[] validItems = new int[700];

        /// <summary>Wether mod is active or not.</summary>
        private static bool IsModActive = true;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            ModHelper = helper;
            ModMonitor = this.Monitor;
            //Read config
            Config = Helper.ReadConfig<ModConfig>();

            Helper.Events.Input.ButtonPressed += OnButtonPressed;
            Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            Helper.Events.GameLoop.DayStarted += OnDayStarted;

            HarmonyInstance harmony = HarmonyInstance.Create(ModManifest.UniqueID);
            //patch resource drops
            harmony.Patch(
                original: AccessTools.Method(typeof(ResourceClump), nameof(ResourceClump.performToolAction)),
                prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.performToolAction_Prefix))
            );

            //patch tree drops
            harmony.Patch(
                original: AccessTools.Method(typeof(Tree), nameof(Tree.performToolAction)),
                prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.performToolAction_Prefix2))
            );
            harmony.Patch(
            original: AccessTools.Method(typeof(Tree), nameof(Tree.tickUpdate)),
            prefix: new HarmonyMethod(typeof(ModEntry), nameof(ModEntry.tickUpdate_Prefix))
            );
        }

        public void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            //Show error message if mod is deactivated
            if (!IsModActive)
            {
                Game1.addHUDMessage(new HUDMessage(ModHelper.Translation.Get("mod-deactivated", new { ModName = ModManifest.Name}), 3));
            }
            /*
            // Just test stuff
            foreach (GameLocation location in Game1.locations)
            {
                foreach (KeyValuePair<Vector2, StardewValley.Object> obj in location.Objects.Pairs)
                {
                    //if(!(obj.Value is Fence) && !(obj.Value is StardewValley.Objects.Chest) && obj.Value.Type != null &&!(obj.Value.Type.Contains("Basic")) && !obj.Value.Type.Contains("asdf"))
                    if (!(obj.Value is Fence) && obj.Value.Type != null && obj.Value.Type.Contains("Crafting"))
                    {
                        ModMonitor.Log($"found at {location.Name}: {obj.Value} {obj.Value.Name} {obj.Value.ParentSheetIndex} {obj.Value.bigCraftable} {obj.Value.Type} {obj.Value.MinutesUntilReady}");
                    }
                }
            }
            */
        }

        /// <summary>
        /// Raised when game is launched
        /// </summary>
        /// <remarks>Used to get valid item IDs</remarks>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event arguments</param>
        public void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            int i = 0;
            foreach (KeyValuePair<int, string> index in Game1.objectInformation)
            {
                //ModMonitor.Log($"key: {index.Key} value: {index.Value}");
                validItems[i] = index.Key;
                i++;
            }
            ModMonitor.Log($"Amount of valid items: {i}");
            CheckConfig();
        }

        /// <summary>
        /// Check Configs for valid values
        /// </summary>
        public void CheckConfig()
        {
            //iterate through all the entries in ResourceDrops section of config
            foreach (KeyValuePair<string, ConfigResorceDrops> index in Config.ResourceDrops)
            {
                //Skip validation of Drops if it's null
                if (Config.ResourceDrops[index.Key].Drops != null)
                {
                    for (int i = 0; i < Config.ResourceDrops[index.Key].Drops.Length / 2; i++)
                    {
                        if (!IsValidItem(Config.ResourceDrops[index.Key].Drops[i, 0]))
                        {
                            ModMonitor.Log($"Error in config: {Config.ResourceDrops[index.Key].Drops[i, 0]} is not a valid item!", LogLevel.Error);
                            IsModActive = false;
                        }
                        if (Config.ResourceDrops[index.Key].Drops[i, 1] < 0) Config.ResourceDrops[index.Key].Drops[i, 1] = 0;
                    }
                }
            }
            //iterate through all the entries in TreeDrops section of config
            foreach (KeyValuePair<string, ConfigTreeDrops> index in Config.TreeDrops)
            {
                //Skip validation of BushDrops if it's null
                if (Config.TreeDrops[index.Key].BushDrops != null) {
                    for (int i = 0; i < Config.TreeDrops[index.Key].BushDrops.Length / 2; i++)
                    {
                        if (!IsValidItem(Config.TreeDrops[index.Key].BushDrops[i, 0]))
                        {
                            ModMonitor.Log($"Error in config: {Config.TreeDrops[index.Key].BushDrops[i, 0]} is not a valid item!", LogLevel.Error);
                            IsModActive = false;
                        }
                        if (Config.TreeDrops[index.Key].BushDrops[i, 1] < 0)
                        {
                            Config.TreeDrops[index.Key].BushDrops[i, 1] = 0;
                        }
                    }
                }
                //Skip validation if of SeedDrops if it's null
                if (Config.TreeDrops[index.Key].SeedDrops != null)
                {
                    for (int i = 0; i < Config.TreeDrops[index.Key].SeedDrops.Length / 2; i++)
                    {
                        if (!IsValidItem(Config.TreeDrops[index.Key].SeedDrops[i, 0]))
                        {
                            ModMonitor.Log($"Error in config: {Config.TreeDrops[index.Key].SeedDrops[i, 0]} is not a valid item!", LogLevel.Error);
                            IsModActive = false;
                        }
                        if (Config.TreeDrops[index.Key].SeedDrops[i, 1] < 0)
                        {
                            Config.TreeDrops[index.Key].SeedDrops[i, 1] = 0;
                        }
                    }
                }
                //Skip validation of SproutDrops if it's null
                if (Config.TreeDrops[index.Key].SproutDrops != null)
                {
                    for (int i = 0; i < Config.TreeDrops[index.Key].SproutDrops.Length / 2; i++)
                    {
                        if (!IsValidItem(Config.TreeDrops[index.Key].SproutDrops[i, 0]))
                        {
                            ModMonitor.Log($"Error in config: {Config.TreeDrops[index.Key].SproutDrops[i, 0]} is not a valid item!", LogLevel.Error);
                            IsModActive = false;
                        }
                        if (Config.TreeDrops[index.Key].SproutDrops[i, 1] < 0)
                        {
                            Config.TreeDrops[index.Key].SproutDrops[i, 1] = 0;
                        }
                    }
                }
                //Skip validation of TreeDrops if it's null
                if (Config.TreeDrops[index.Key].TreeDrops != null) {
                    for (int i = 0; i < Config.TreeDrops[index.Key].TreeDrops.Length / 2; i++)
                    {
                        if (!IsValidItem(Config.TreeDrops[index.Key].TreeDrops[i, 0]))
                        {
                            ModMonitor.Log($"Error in config: {Config.TreeDrops[index.Key].TreeDrops[i, 0]} is not a valid item!", LogLevel.Error);
                            IsModActive = false;
                        }
                        if (Config.TreeDrops[index.Key].TreeDrops[i, 1] < 0)
                        {
                            Config.TreeDrops[index.Key].TreeDrops[i, 1] = 0;
                        }
                    }
                }
                //Skip validation of StumpDrops if it's null
                if (Config.TreeDrops[index.Key].StumpDrops != null)
                {
                    for (int i = 0; i < Config.TreeDrops[index.Key].StumpDrops.Length / 2; i++)
                    {
                        if (!IsValidItem(Config.TreeDrops[index.Key].StumpDrops[i, 0]))
                        {
                            ModMonitor.Log($"Error in config: {Config.TreeDrops[index.Key].StumpDrops[i, 0]} is not a valid item!", LogLevel.Error);
                            IsModActive = false;
                        }
                        if (Config.TreeDrops[index.Key].StumpDrops[i, 1] < 0)
                        {
                            Config.TreeDrops[index.Key].StumpDrops[i, 1] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if item is a valid item
        /// </summary>
        /// <returns><c>true</c>, if item is valid, <c>false</c> otherwise.</returns>
        /// <param name="item">The ID of the item to check</param>
        public bool IsValidItem (int item)
        {
            for (int i = 0; i < validItems.Length; i++)
            {
                if (item == validItems[i])
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Gets the key for for an object.
        /// </summary>
        /// <returns>The key for an object.</returns>
        /// <param name="index">parentSheetIndex of ResourceClump or treeType</param>
        public static string GetKeyForObject(int index)
        {
            switch (index)
            {
                case 1:
                    return "OakTree";
                case 2:
                    return "MapleTree";
                case 3:
                    return "PineTree";
                case 4:
                    return "WinterOak";
                case 5:
                    return "WinterMaple";
                case 6:
                    return "PalmTree";
                case 7:
                    return "MushroomTree";
                case 600:
                    return "LargeStump";
                case 602:
                    return "LargeLog";
                case 622:
                    return "Meteorite";
                case 672:
                    return "Boulder";
                case 752:
                    return "MineRock1";
                case 754:
                    return "MineRock2";
                case 756:
                    return "MineRock3";
                case 758:
                    return "MineRock4";
            }
            return "";
        }

        /// <summary>
        /// Gets called when a button is pressed 
        /// </summary>
        /// <remarks>Useless. Was intended for a non-harmony version of this mod</remarks>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event argument</param>
        public void OnButtonPressed (object sender, ButtonPressedEventArgs e)
        {
            /*
            if (!Context.IsPlayerFree) return;
            SButton key = e.Button;
            if (key.IsUseToolButton())
            {
                Tool t = Game1.player.CurrentTool;
                if (Game1.currentLocation.Name.Equals("Farm")) 
                {
                    ModMonitor.Log("player used tool at farm");
                }
                else if(Game1.currentLocation.Name.Equals("Forest"))
                {
                    ModMonitor.Log("player used tool at forest");
                }
                else if(Game1.currentLocation.Name.Equals("Woods"))
                {
                    ModMonitor.Log("player used tool in woods");
                }
                else
                {
                    ModMonitor.Log($"player used tool in {Game1.currentLocation.Name}");
                }
            }
            */
        }

        /// <summary>"hook" of performToolAction in StardewValley.TerrainFeatures.ResourceClump</summary>
        /// <param name="__instance">reference to acces values in ResourceClump class</param>
        /// <param name="t">the tool used</param>
        /// <param name="damage">Damage to player/object? (Not used)</param>
        /// <param name="tileLocation">Location of the tile.</param>
        /// <param name="location">Location where the action is performed.</param>
        /// <returns><c>true</c>run default game code, <c>false</c>prevent gamecode from running</returns>
        // Basically runs the same code twice if a large stump or large log breaks, resulting in twice the amount of hardwood
        public static bool performToolAction_Prefix(ResourceClump __instance, Tool t, int damage, Vector2 tileLocation, GameLocation location)
        {
            //Don't do anything if mod isn't active
            if (!IsModActive)
            {
                return true;
            }
            try
            {
                // define neccessary variables 
                NetInt height = __instance.height;
                NetInt width = __instance.width;
                NetFloat health = __instance.health;
                NetInt parentSheetIndex = __instance.parentSheetIndex;
                string key = GetKeyForObject(parentSheetIndex);
                ModMonitor.Log($"performToolAction: key: {key}, index: {parentSheetIndex}");
                //Skip if Drops is null
                if (Config.ResourceDrops[key].Drops == null)
                {
                    return true;
                }
                //Taken from the game code
                float num = Math.Max(1f, (float)((int)t.upgradeLevel + 1) * 0.75f);
                health.Value -= num;
                //Game1.createRadialDebris(Game1.currentLocation, debrisType, (int)tileLocation.X + Game1.random.Next((int)width / 2 + 1), (int)tileLocation.Y + Game1.random.Next((int)height / 2 + 1), Game1.random.Next(4, 9), false, -1, false, -1);

                if ((float)health <= 0f)
                {
                    //The important stuff
                    switch ((int)parentSheetIndex)
                    {
                        //Large Stump, Large Log
                        case 600:
                        case 602:
                            {
                                for (int i = 0; i < Config.ResourceDrops[key].Drops.Length / 2; i++)
                                {
                                    if (Config.ResourceDrops[key].Drops[i, 1] != 0)
                                    {
                                        if (Game1.IsMultiplayer)
                                        {
                                            Game1.createMultipleObjectDebris(Config.ResourceDrops[key].Drops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.ResourceDrops[key].Drops[i, 1], t.getLastFarmerToUse().UniqueMultiplayerID);
                                        }
                                        else
                                        {
                                            Game1.createMultipleObjectDebris(Config.ResourceDrops[key].Drops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.ResourceDrops[key].Drops[i, 1]);
                                        }
                                    }
                                }
                                return true; //run default code anyway
                            }
                        //Boulder, MineRock1, MineRock2, MineRock3, MineRock4
                        case 672:
                        case 752:
                        case 754:
                        case 756:
                        case 758:
                            {
                                for (int i = 0; i < Config.ResourceDrops[key].Drops.Length / 2; i++)
                                {
                                    if (Game1.IsMultiplayer)
                                    {
                                        Game1.createMultipleObjectDebris(Config.ResourceDrops[key].Drops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.ResourceDrops[key].Drops[i, 1], t.getLastFarmerToUse().UniqueMultiplayerID);
                                    }
                                    else
                                    {
                                        Game1.createRadialDebris(Game1.currentLocation, Config.ResourceDrops[key].Drops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.ResourceDrops[key].Drops[i, 1], false, -1, true, -1);
                                    }
                                }
                                return true;
                            }
                        //Meteorite
                        case 622:
                            {
                                for (int i = 0; i < Config.ResourceDrops[key].Drops.Length / 2; i++)
                                {
                                    if (Config.ResourceDrops[key].Drops[i, 1] != 0)
                                    {
                                        if (Game1.IsMultiplayer)
                                        {
                                            Game1.createMultipleObjectDebris(Config.ResourceDrops[key].Drops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.ResourceDrops[key].Drops[i, 1], t.getLastFarmerToUse().UniqueMultiplayerID);
                                        }
                                        else
                                        {
                                            Game1.createMultipleObjectDebris(Config.ResourceDrops[key].Drops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.ResourceDrops[key].Drops[i, 1]);
                                        }
                                    }
                                }
                                return true;
                            }
                    }
                }
                //Avoid more damage to objects than intended
                else health.Value += num;
                return true;
            }
            catch (Exception ex)
            {
                ModMonitor.Log($"Failed in {nameof(performToolAction_Prefix)}:\n{ex}", LogLevel.Error);
                return true; // run original logic
            }
        }
        /// <summary>
        /// "hook" of performToolAction in StardewValley.TerrainFeatures.Tree
        /// </summary>
        /// <returns><c>true</c> proceed to game code <c>false</c> stop game code</returns>
        /// <param name="__instance"> reference to access variables in Tree.</param>
        /// <param name="t">Tool used</param>
        /// <param name="explosion">Explosion.</param>
        /// <param name="tileLocation">Tile location.</param>
        /// <param name="location">Location where action is performed</param>
        private static bool performToolAction_Prefix2(Tree __instance, Tool t, int explosion, Vector2 tileLocation, GameLocation location)
        {
            //Don't do anything if Mod isn't active
            if (!IsModActive)
            {
                return true;
            }
            ModMonitor.Log($"Prefix2 called with: t:{t} explosion:{explosion} tileLocation: {tileLocation} location: {location}");
            try
            {
                //iterates through all terrain features at the location
                foreach (KeyValuePair<Vector2, TerrainFeature> pair in location.terrainFeatures.Pairs)
                {
                    if (pair.Key == tileLocation)
                    {
                        ModMonitor.Log($"found {pair.Value}!", LogLevel.Trace);
                        if (pair.Value is Tree)
                        {
                            ModMonitor.Log("It's a tree!", LogLevel.Trace);
                            tree = (Tree)pair.Value;
                        }
                    }

                }
                //NOTE: Using net types will *totally* not fuck up everything in multiplayer
                NetBool tapped = tree.tapped;
                NetFloat health = tree.health;
                NetInt growthStage = tree.growthStage;
                NetBool stump = tree.stump;
                //NetInt treeType = tree.treeType;

                ModMonitor.Log($"Tree properties: growthStage: {tree.growthStage} stump: {tree.stump} tapped: {tree.tapped} treeType: {tree.treeType} shakeLeft: {tree.shakeLeft}");

                if (location == null)
                {
                    location = Game1.currentLocation;
                }
                if (explosion > 0)
                {
                    tapped.Value = false;
                }

                if ((bool)tapped)
                {
                    return true;
                }
                if ((float)health <= -99f)
                {
                    return true;
                }

                if ((int)growthStage >= 5)
                {
                    if (t != null && t is Axe)
                    {

                                          
                        //location.playSound("axchop", NetAudio.SoundContext.Default);
                        //lastPlayerToHit.Value = t.getLastFarmerToUse().UniqueMultiplayerID;
                        //location.debris.Add(new Debris(12, Game1.random.Next(1, 3), t.getLastFarmerToUse().GetToolLocation(false) + new Vector2(16f, 0f), t.getLastFarmerToUse().Position, 0, ((int)treeType == 7) ? 10000 : (-1)));
                        /*
                        // try to create secret note 
                        if (!(bool)stump && t.getLastFarmerToUse() != null && t.getLastFarmerToUse().hasMagnifyingGlass && Game1.random.NextDouble() < 0.005)
                        {
                            Object @object = location.tryToCreateUnseenSecretNote(t.getLastFarmerToUse());
                            if (@object != null)
                            {
                                Game1.createItemDebris(@object, new Vector2(tileLocation.X, tileLocation.Y - 3f) * 64f, -1, location, Game1.player.getStandingY() - 32);
                            }
                        }
                        */
                    }
                    else if (explosion <= 0)
                    {
                        return true;
                    }
                    //shake(tileLocation, true, location);
                    float num = 1f;
                    if (explosion > 0)
                    {
                        num = (float)explosion;
                    }
                    else
                    {
                        if (t == null)
                        {
                            return true;
                        }
                        switch ((int)t.upgradeLevel)
                        {
                            //Starter tool
                            case 0:
                                num = 1f;
                                break;
                            //Copper tool
                            case 1:
                                num = 1.25f;
                                break;
                            //Steel (Iron) Tool
                            case 2:
                                num = 1.67f;
                                break;
                            //Gold tool
                            case 3:
                                num = 2.5f;
                                break;
                            //Iridium tool
                            case 4:
                                num = 5f;
                                break;
                        }
                    }
                    health.Value -= num;
                    if ((float)health <= 0f && performTreeFall(t, explosion, tileLocation, location))
                    {
                        return true;
                    }
                    health.Value += num;
                }
                else if ((int)growthStage >= 3)
                {
                    if (t != null && t.BaseName.Contains("Ax"))
                    {
                        /*
                        NetCollection<Debris> debris = location.debris;
                        int numberOfChunks = Game1.random.Next((int)t.upgradeLevel * 2, (int)t.upgradeLevel * 4);
                        Vector2 debrisOrigin = t.getLastFarmerToUse().GetToolLocation(false) + new Vector2(16f, 0f);
                        Rectangle boundingBox = t.getLastFarmerToUse().GetBoundingBox();
                        float x = (float)boundingBox.Center.X;
                        boundingBox = t.getLastFarmerToUse().GetBoundingBox();
                        debris.Add(new Debris(12, numberOfChunks, debrisOrigin, new Vector2(x, (float)boundingBox.Center.Y), 0, -1));
                        */
                    }
                    else if (explosion <= 0)
                    {
                        return true;
                    }
                    //shake(tileLocation, true, location);
                    float num2 = 1f;
                    /*
                    if (Game1.IsMultiplayer)
                    {
                        Random recentMultiplayerRandom = Game1.recentMultiplayerRandom;
                    }
                    else
                    {
                        new Random((int)((float)(double)Game1.uniqueIDForThisGame + tileLocation.X * 7f + tileLocation.Y * 11f + (float)(double)Game1.stats.DaysPlayed + (float)health));
                    }
                    */
                    if (explosion <= 0)
                    {
                        switch ((int)t.upgradeLevel)
                        {
                            case 0:
                                num2 = 2f;
                                break;
                            case 1:
                                num2 = 2.5f;
                                break;
                            case 2:
                                num2 = 3.34f;
                                break;
                            case 3:
                                num2 = 5f;
                                break;
                            case 4:
                                num2 = 10f;
                                break;
                        }
                    }
                    else
                    {
                        num2 = (float)explosion;
                    }
                    health.Value -= num2;
                    if ((float)health <= 0f)
                    {
                        performBushDestroy(tileLocation, location);
                        return true;
                    }
                    health.Value += num2;
                }
                else if ((int)growthStage >= 1)
                {
                    /*
                    if (explosion > 0)
                    {
                        location.playSound("cut", NetAudio.SoundContext.Default);
                        return true;
                    }
                    if (t != null && t.BaseName.Contains("Axe"))
                    {
                        location.playSound("axchop", NetAudio.SoundContext.Default);
                        Game1.createRadialDebris(location, 12, (int)tileLocation.X, (int)tileLocation.Y, Game1.random.Next(10, 20), false, -1, false, -1);
                    }
                    */
                    if (t is Axe || t is Pickaxe || t is Hoe || t is MeleeWeapon)
                    {
                        performSproutDestroy(t, tileLocation, location);
                        return true;
                    }
                }
                else
                {
                    if (explosion > 0)
                    {
                        return true;
                    }
                    if (t.BaseName.Contains("Axe") || t.BaseName.Contains("Pick") || t.BaseName.Contains("Hoe"))
                    {
                        performSeedDestroy(t, tileLocation, location);
                        return true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ModMonitor.Log($"Failed in {nameof(performToolAction_Prefix2)}:\n{ex}", LogLevel.Error);
                return true; // run original logic
            }
        }
        /// <summary>
        /// "hook" of tickUpdate in StardewValley.TerrainFeatures.Tree
        /// </summary>
        /// <remarks>Warning: Gets called a lot</remarks>
        /// <returns><c>true</c> continue game code, <c>false</c> otherwise.</returns>
        /// <param name="time">current GameTime</param>
        /// <param name="tileLocation">location of tile to update</param>
        /// <param name="location">GameLocation</param>
        private static bool tickUpdate_Prefix (GameTime time, Vector2 tileLocation, GameLocation location)
        {
            //Skip if Mod isn't active
            if (!IsModActive)
            {
                return true;
            }
            try
            {
                //skip if tree is unset or tileLocation isn't location of tree
                if(tree == null || !tileLocation.Equals(tree.currentTileLocation))
                {
                    return true;
                }

                //ModMonitor.Log($"tickUpdate_Prefix called with: time: {time} tileLocation: {tileLocation} location: {location}");
                NetBool falling = ModHelper.Reflection.GetField<NetBool>(tree, "falling").GetValue();
                NetBool destroy = ModHelper.Reflection.GetField<NetBool>(tree, "destroy").GetValue();

                if ((bool)destroy)
                {
                    return true;
                }

                if ((bool)falling)
                {
                    NetBool shakeLeft = tree.shakeLeft;
                    float shakeRotation = ModHelper.Reflection.GetField<float>(tree, "shakeRotation").GetValue();
                    float maxShake = ModHelper.Reflection.GetField<float>(tree, "maxShake").GetValue();

                    shakeRotation += (((bool)shakeLeft) ? (0f - maxShake * maxShake) : (maxShake * maxShake));
                    maxShake += 0.00153398083f;

                    if ((double)Math.Abs(shakeRotation) > 1.5707963267948966)
                    {
                        NetInt treeType = tree.treeType;
                        NetLong lastPlayerToHit = ModHelper.Reflection.GetField<NetLong>(tree, "lastPlayerToHit").GetValue();

                        string key = GetKeyForObject(treeType);

                        if (Config.TreeDrops[key].TreeDrops != null)
                        {
                            for (int i = 0; i < Config.TreeDrops[key].TreeDrops.Length / 2; i++)
                            {
                                if(Game1.IsMultiplayer)
                                {
                                    Game1.createMultipleObjectDebris(Config.TreeDrops[key].TreeDrops[i, 0], (int)tileLocation.X + (((bool)shakeLeft) ? (-4) : 4), (int)tileLocation.Y, Config.TreeDrops[key].TreeDrops[i, 1], lastPlayerToHit, location);
                                }
                                else
                                {
                                    Game1.createMultipleObjectDebris(Config.TreeDrops[key].TreeDrops[i, 0], (int)tileLocation.X + (((bool)shakeLeft) ? (-4) : 4), (int)tileLocation.Y, Config.TreeDrops[key].TreeDrops[i, 1], location);
                                }
                            }
                        }
                    }
                    else
                    {
                        maxShake -= 0.00153398083f;
                        shakeRotation -= (((bool)shakeLeft) ? (0f - maxShake * maxShake) : (maxShake * maxShake));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ModMonitor.Log($"Failed in {nameof(tickUpdate_Prefix)}:\n{ex}", LogLevel.Error);
                return true; // run original logic
            }
        }
        /// <summary>
        /// Performs the tree fall.
        /// </summary>
        /// <returns><c>true</c>, if tree fall was performed, <c>false</c> otherwise.</returns>
        /// <param name="t">Tool used</param>
        /// <param name="explosion">Explosion. (Not used)</param>
        /// <param name="tileLocation">Tile location.</param>
        /// <param name="location">Location.</param>
        public static bool performTreeFall(Tool t, int explosion, Vector2 tileLocation, GameLocation location)
        {
            ModMonitor.Log($"performTreeFall called with: t: {t} explosion: {explosion} tileLocation: {tileLocation} location: {location}", LogLevel.Trace);

            NetFloat health = tree.health;
            NetBool stump = tree.stump;
            NetInt treeType = tree.treeType;
            NetBool shakeLeft = tree.shakeLeft;

            NetLong lastPlayerToHit = ModHelper.Reflection.GetField<NetLong>(tree, "lastPlayerToHit").GetValue();
            NetBool falling = ModHelper.Reflection.GetField<NetBool>(tree, "falling").GetValue();

            string key = GetKeyForObject(treeType);

            //Code for calculation of tree fall position: (int)tileLocation.X + (((bool)shakeLeft) ? (-4) : 4)
            if ((bool)stump)
            {
                if (Config.TreeDrops[key].StumpDrops != null)
                {
                    for (int i = 0; i < Config.TreeDrops[key].StumpDrops.Length / 2; i++)
                    {
                        if (Game1.IsMultiplayer)
                        {
                            Game1.createMultipleObjectDebris(Config.TreeDrops[key].StumpDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].StumpDrops[i, 1], lastPlayerToHit, location);
                        }
                        else
                        {
                            Game1.createMultipleObjectDebris(Config.TreeDrops[key].StumpDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].StumpDrops[i, 1], location);
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Performs the bush destroy.
        /// </summary>
        /// <param name="tileLocation">Tile location.</param>
        /// <param name="location">Location.</param>
        private static void performBushDestroy(Vector2 tileLocation, GameLocation location)
        {
            ModMonitor.Log($"performBushDestroy called with: tileLocation: {tileLocation} location: {location}");

            NetInt treeType = tree.treeType;
            NetLong lastPlayerToHit = ModHelper.Reflection.GetField<NetLong>(tree, "lastPlayerToHit").GetValue();

            string key = GetKeyForObject(treeType);

            if (Config.TreeDrops[key].BushDrops != null)
            {
                for (int i = 0; i < Config.TreeDrops[key].BushDrops.Length/2; i++)
                {
                    if (Game1.IsMultiplayer)
                    {
                        Game1.createMultipleObjectDebris(Config.TreeDrops[key].BushDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].BushDrops[i, 1], lastPlayerToHit, location);
                    }
                    else
                    {
                        Game1.createMultipleObjectDebris(Config.TreeDrops[key].BushDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].BushDrops[i, 1], location);
                    }
                }
            }
        }
        /// <summary>
        /// Performs the seed destroy.
        /// </summary>
        /// <param name="t">Tool used</param>
        /// <param name="tileLocation">Tile location.</param>
        /// <param name="location">Location.</param>
        private static void performSeedDestroy(Tool t, Vector2 tileLocation, GameLocation location)
        {
            ModMonitor.Log($"performSeddDestroy calles with: t: {t} tileLocation: {tileLocation} location: {location}");

            NetInt treeType = tree.treeType;
            NetLong lastPlayerToHit = ModHelper.Reflection.GetField<NetLong>(tree, "lastPlayerToHit").GetValue();

            string key = GetKeyForObject(treeType);

            if (Config.TreeDrops[key].SeedDrops != null)
            {
                for (int i = 0; i < Config.TreeDrops[key].SeedDrops.Length/2; i++)
                {
                    if (Game1.IsMultiplayer)
                    {
                        Game1.createMultipleObjectDebris(Config.TreeDrops[key].SeedDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].SeedDrops[i, 1], lastPlayerToHit, location);
                    }
                    else
                    {
                        Game1.createMultipleObjectDebris(Config.TreeDrops[key].SeedDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].SeedDrops[i, 1], location);
                    }
                }
            }
        }
        /// <summary>
        /// Performs the sprout destroy.
        /// </summary>
        /// <param name="t">Tool used</param>
        /// <param name="tileLocation">Tile location.</param>
        /// <param name="location">Location.</param>
        private static void performSproutDestroy(Tool t, Vector2 tileLocation, GameLocation location)
        {
            ModMonitor.Log($"performSproutDestroy called with: t: {t} tileLocation: {tileLocation} location: {location}");

            NetInt treeType = tree.treeType;

            string key = GetKeyForObject(treeType);

            if (Config.TreeDrops[key].SproutDrops != null)
            {
                for (int i = 0; i < Config.TreeDrops[key].SproutDrops.Length/2; i++)
                {
                    if (Game1.IsMultiplayer)
                    {
                        NetLong lastPlayerToHit = ModHelper.Reflection.GetField<NetLong>(tree, "lastPlayerToHit").GetValue();
                        Game1.createMultipleObjectDebris(Config.TreeDrops[key].SproutDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].SproutDrops[i, 1], lastPlayerToHit, location);
                    }
                    else
                    {
                        Game1.createMultipleObjectDebris(Config.TreeDrops[key].SproutDrops[i, 0], (int)tileLocation.X, (int)tileLocation.Y, Config.TreeDrops[key].SproutDrops[i, 1], location);
                    }
                }
            }
        }

    }
}
