using System;
using System.Collections.Generic;
using StardewModdingAPI;


namespace MoreHardwood
{
    class ConfigResorceDrops
    {
        public int[,] Drops { get; set; }
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
        public Dictionary<string, ConfigResorceDrops> ResourceDrops { get; set; } = new Dictionary<string, ConfigResorceDrops>
        {
            //Large Stump
            ["LargeStump"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {709, 2}
                }
            },
            //Large Log
            ["LargeLog"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {709, 4}
                }
            },
            //Meteorite
            ["Meteorite"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {386, 6},
                    {390, 6},
                    {535, 2}
                }
            },
            //Boulder
            ["Boulder"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {390, 15}
                }
            },
            //MineRock1
            ["MineRock1"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {390, 10}
                }
            },
            //MineRock2
            ["MineRock2"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {390, 10}
                }
            },
            //MineRock3
            ["MineRock3"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {390, 10}
                }
            },
            //MineRock4
            ["MineRock4"] = new ConfigResorceDrops
            {
                Drops = new int[,]
                {
                    {390, 10}
                }
            }
        };
        public Dictionary<string, ConfigTreeDrops> TreeDrops { get; set; } = new Dictionary<string, ConfigTreeDrops>
        {
            //bushyTree (Oak)
            ["OakTree"] = new ConfigTreeDrops
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
            ["MapleTree"] = new ConfigTreeDrops
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
            ["PineTree"] = new ConfigTreeDrops
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
            ["WinterOak"] = new ConfigTreeDrops
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
            ["WinterMaple"] = new ConfigTreeDrops
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
            ["PalmTree"] = new ConfigTreeDrops
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
            ["MushroomTree"] = new ConfigTreeDrops
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
