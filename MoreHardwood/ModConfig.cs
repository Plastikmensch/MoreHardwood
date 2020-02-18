using System;
using System.Collections.Generic;
using StardewModdingAPI;

namespace MoreHardwood
{
    class ConfigResorceDrops
    {
        public int[] ItemID { get; set; }
        public int[] Amount { get; set; }
    }
    class ConfigTreeDrops
    {
        public int[,] SeedDrops { get; set; }
        public int[,] SproutDrops { get; set; }
        public int[,] BushDrops { get; set; }
        public int[,] TreeDrops { get; set; }
        public int[,] StumpDrops { get; set; }
    }
    internal class ModConfig
    {
        //public int hardwood { get; set; } = 2;
        public Dictionary<int, ConfigResorceDrops> ResourceDrops { get; set; } = new Dictionary<int, ConfigResorceDrops> 
        {
            //Large Stump
            [600] = new ConfigResorceDrops
            {
                ItemID = new int[] { 709 }, Amount = new int[] { 2 }
            },
            //Large Log
            [602] = new ConfigResorceDrops
            {
                ItemID = new int[] { 709 }, Amount = new int[] { 8 }
            },
            //Meteorite
            [622] = new ConfigResorceDrops
            {
                ItemID = new int[] { 386, 390, 535}, Amount = new int[] {6, 6, 2}
            },
            //Boulder
            [672] = new ConfigResorceDrops
            {
                ItemID = new int[] {390}, Amount = new int[] {15}
            },
            //MineRock1
            [752] = new ConfigResorceDrops
            {
                ItemID = new int[] { 390 },
                Amount = new int[] { 10 }
            },
            //MineRock2
            [754] = new ConfigResorceDrops
            {
                ItemID = new int[] { 390 },
                Amount = new int[] { 10 }
            },
            //MineRock3
            [756] = new ConfigResorceDrops
            {
                ItemID = new int[] { 390 },
                Amount = new int[] { 10 }
            },
            //MineRock4
            [758] = new ConfigResorceDrops
            {
                ItemID = new int[] { 390 },
                Amount = new int[] { 10 }
            }
        };
        public Dictionary<int, ConfigTreeDrops> TreeDrops { get; set; } = new Dictionary<int, ConfigTreeDrops>
        {
            //bushyTree (Oak)
            [1] = new ConfigTreeDrops
            {
                SeedDrops = new int[,]
                {
                    {309, 1}
                },
                SproutDrops = new int[,]
                {
                    {388, 1}
                },
                BushDrops = new int[,]
                {
                    {388, 4}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {388, 5}
                },
                StumpDrops = new int[,]
                {
                    {388, 4}
                }
            },
            //leafyTree (Maple)
            [2] = new ConfigTreeDrops
            {
                SeedDrops = new int[,]
                {
                    {310, 1}
                },
                SproutDrops = new int[,]
                {
                    {388, 1}
                },
                BushDrops = new int[,]
                {
                    {388, 4}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {388, 5}
                },
                StumpDrops = new int[,]
                {
                    {388, 4}
                }
            },
            //pineTree (Pine)
            [3] = new ConfigTreeDrops
            {
                SeedDrops = new int[,]
                {
                    {311, 1}
                },
                SproutDrops = new int[,]
                {
                    {388, 1}
                },
                BushDrops = new int[,]
                {
                    {388, 4}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {388, 5}
                },
                StumpDrops = new int[,]
                {
                    {388, 4}
                }
            },
            //winterTree1 / bushyTree (Oak in winter)
            [4] = new ConfigTreeDrops
            {
                SeedDrops = new int[,]
                {
                    {309, 1}
                },
                SproutDrops = new int[,]
                {
                    {388, 1}
                },
                BushDrops = new int[,]
                {
                    {388, 4}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {388, 5}
                },
                StumpDrops = new int[,]
                {
                    {388, 4}
                }
            },
            //winterTree2 / leafyTree (Maple in winter)
            [5] = new ConfigTreeDrops
            {
                SeedDrops = new int[,]
                {
                    {310, 1}
                },
                SproutDrops = new int[,]
                {
                    {388, 1}
                },
                BushDrops = new int[,]
                {
                    {388, 4}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {388, 5}
                },
                StumpDrops = new int[,]
                {
                    {388, 4}
                }
            },
            //palmTree
            [6] = new ConfigTreeDrops
            {
                SproutDrops = new int[,]
                {
                    {388, 1}
                },
                BushDrops = new int[,]
                {
                    {388, 4}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {388, 5}
                },
                StumpDrops = new int[,]
                {
                    {388, 6}
                }
            },
            //mushroomTree
            [7] = new ConfigTreeDrops
            {
                BushDrops = new int[,]
                {
                    {420, 1}
                },
                TreeDrops = new int[,]
                {
                    {92, 2},
                    {420, 5},
                    {422, 5}
                },
                StumpDrops = new int[,]
                {
                    {420, 1},
                    {422, 1}
                }
            }
        };
    }
}
