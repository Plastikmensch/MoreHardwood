# MoreHardwood
Stardew Valley Mod to increase item drops

## What this does
Originally intended to only increase the amount of Hardwood dropped by Large Logs and Large Stumps (hence the name), but can be used to alter all kinds of drops from resources and trees.
Example: Cut a Large Stump, get 2 base Hardwood from the game and the items defined in the config (default: 2 Hardwood)

## Configuration
Config consists of basically 2 parts: ResourceDrops and TreeDrops
### ResourceDrops
ResourceDrops has as key the name of an ResourceClump. Every key has as value an array (Drops), which defines which and how much additional items should drop from a Resourceclump. Example: ```"LargeStump": {
      "Drops": [
        [
          709,
          2
        ]
      ]
    }``` This example means that Large Stumps drop 2 additional Hardwood (ID: 709)
    Drops can also be null (```"Drops": null), so no additional items will drop.
### TreeDrops
Treedrops has as key the type of tree as string. Each key itself has a key for the growth stage of the tree.
Example: ```"OakTree": {
      "SeedDrops": [
        [
          309,
          1
        ]
      ],``` Means that an oak tree, which is just a seed, drops 1 additional Acorn (ID: 309)