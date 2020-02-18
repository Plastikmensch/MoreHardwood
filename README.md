# MoreHardwood
Stardew Valley Mod to increase item drops

## What this does
Originally intended to only increase the amount of Hardwood dropped by Large Logs and Large Stumps (hence the name), but can be used to alter all kinds of drops from resources and trees.
Example: Cut a Large Stump, get 2 base Hardwood from the game and the items defined in the config (default: 2 Hardwood)

## Configuration
(It's complicated, I know...)<br>
Config consists of basically 2 parts: ResourceDrops and TreeDrops
### ResourceDrops
ResourceDrops has as key the parentSheetIndex of Objects. Meaning: "600"=Large Stump, "602"= Large Log etc.
Every key has 2 Arrays as value: ItemID and Amount.
ItemID are the IDs of items which should drop and Amount are the amount of how many should drop.
Example: ```"600": {
      "ItemID": [
        709
      ],
      "Amount": [
        2
      ]
    },``` Means Large Stumps ("600") drop 2 (Amount: 2) Hardwood(ItemID: 709)
### TreeDrops
TreeDrops has as key the type of tree (1 = Oak Tree, 2 = Maple Tree etc.).
Every key has other keys for the stage of the tree. Meaning: SeedDrops = What tree seed drop, TreeDrops = What a full grown tree drops, etc.
The value for every tree stage are arrays in the [ItemID, Amount] format.
Example:```"TreeDrops": {
    "1": {
      "SeedDrops": [
        [
          309,
          1
        ]
      ],``` Oak Trees ("1") drop, when only a seed, one acorn (309, 1)