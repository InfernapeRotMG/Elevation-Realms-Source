#region

using System;
using System.Collections.Generic;
using System.Linq;
using db.data;
using log4net;

#endregion

namespace wServer.realm.entities
{
    internal class MerchantLists
    {
        public static int[] AccessoryClothList;
        public static int[] AccessoryDyeList;
        public static int[] ClothingClothList;
        public static int[] ClothingDyeList;

        public static Dictionary<int, Tuple<int, CurrencyType>> prices = new Dictionary<int, Tuple<int, CurrencyType>>
        {
            {0xb41, new Tuple<int, CurrencyType>(0, CurrencyType.Fame)},
            {0xbab, new Tuple<int, CurrencyType>(0, CurrencyType.Fame)},
            {0xbad, new Tuple<int, CurrencyType>(0, CurrencyType.Fame)},

            //WEAPONS
            {0xaf6, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xa87, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa86, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa85, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa07, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xb02, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xa8d, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa8c, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa8b, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa1e, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xb08, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xaa2, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xaa1, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xaa0, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa9f, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xb0b, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa47, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa84, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa83, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa82, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xaff, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xa8a, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa89, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa88, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa19, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xc50, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xc4f, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xc4e, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xc4d, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xc4c, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},

            //Rings
            {0xabf, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac0, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xac1, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac2, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac3, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac4, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac5, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac6, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xac7, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xac8, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xac9, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xaca, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xacb, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xacc, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xacd, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xace, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            //ARMORS
            {0xb05, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa96, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xa95, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa94, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa60, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xafc, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa93, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xa92, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa91, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xa13, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xaf9, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa90, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xa8f, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},
            {0xa8e, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xad3, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)},

            //ABILITIES
            {0xb25, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa5b, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb22, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa0c, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb24, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa30, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb26, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa55, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb27, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xae1, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb28, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa65, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb29, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa6b, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb2a, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xaa8, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb2b, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xaaf, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb2c, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xab6, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb2d, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xa46, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb23, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xb20, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xb33, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xb32, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xc59, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xc58, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},

            //PET FOOD
            {0xccc, new Tuple<int, CurrencyType>(5, CurrencyType.Fame)},
            {0xccb, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xcca, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xcc9, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xcc8, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)},
            {0xcc7, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)},
            {0xcc6, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xcc5, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},
            {0xcc4, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)},

            //EGGS
            {0xc86, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon feline egg
            {0xc87, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare feline egg
            {0xc8a, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon canine egg
            {0xc8b, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare canine egg
            {0xc8e, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon avian egg
            {0xc8f, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare avian egg
            {0xc92, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon exotic egg
            {0xc93, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare exotic egg
            {0xc96, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon farm egg
            {0xc97, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare farm egg
            {0xc9a, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon woodland egg
            {0xc9b, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare woodland egg
            {0xc9e, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon reptile egg
            {0xc9f, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare reptile egg
            {0xca2, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon insect egg
            {0xca3, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare insect egg
            {0xca6, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon pinguin egg
            {0xca7, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare pinguin egg
            {0xcaa, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon aquatic egg
            {0xcab, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare aquatic egg
            {0xcae, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon spooky egg
            {0xcaf, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare spooky egg
            {0xcb2, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon humanoid egg
            {0xcb3, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare humanoid egg
            {0xcb6, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon ???? egg
            {0xcb7, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare ???? egg
            {0xcba, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon automaton egg
            {0xcbb, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare automaton egg
            {0xcbe, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //uncommon mystery egg
            {0xcbf, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //rare mystery egg

            //KEYS
            {0x2290, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)}, //Bella's Key - just temponary for testing

            {0x701, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)}, //Undead lair key
            {0x705, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)}, //Pirate cave key
            {0x70a, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //Abyss of demons key
            {0x70b, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)}, //Snake pit key
            {0x710, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //Tomb of the ancients key
            {0x71f, new Tuple<int, CurrencyType>(1, CurrencyType.Fame)}, //Sprite World Key
            {0xc11, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //Ocean Trench Key
            {0xc19, new Tuple<int, CurrencyType>(2, CurrencyType.Fame)}, //Totem Key
            {0xc23, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //Manor Key
            {0xc2e, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //Daby's Key
            {0xc2f, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //Lab Key
            {0xcce, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //Deadwater docks key
            {0xccf, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //Woodland Labyrinth Key
            {0xcda, new Tuple<int, CurrencyType>(4, CurrencyType.Fame)}, //The crawling depths key
            {0xcdd, new Tuple<int, CurrencyType>(3, CurrencyType.Fame)}, //Shatters key
        };

        public static int[] store10List = { 0xb41 };
        public static int[] store11List = { 0xb41 };
        public static int[] store12List = { 0xb41 };
        public static int[] store13List = { 0xb41 };
        public static int[] store14List = { 0xb41 };
        public static int[] store15List = { 0xb41 };
        public static int[] store16List = { 0xb41 };
        public static int[] store17List = { 0xb41 };
        public static int[] store18List = { 0xb41 };
        public static int[] store19List = { 0xb41 };

        public static int[] store1List = { 0xb41 };

        public static int[] store20List = { 0xb41 };

        //keys need to add etcetc
        public static int[] store2List = { 0xb41 };

        //pet eggs
        public static int[] store3List = { 0xb41 };
        //pet food
        public static int[] store4List = { 0xb41 };

        //abilities
        public static int[] store5List = { 0xb41 };

        //armors
        public static int[] store6List = { 0xb41 };

        //Wands&staves&bows
        public static int[] store7List = { 0xb41 };

        //Swords&daggers&samurai shit
        public static int[] store8List = { 0xb41 };

        // rings
        public static int[] store9List = { 0xb41 };

        private static readonly ILog log = LogManager.GetLogger(typeof (MerchantLists));

        public static void InitMerchatLists(XmlData data)
        {
            log.Info("Loading merchant lists...");
            List<int> accessoryDyeList = new List<int>();
            List<int> clothingDyeList = new List<int>();
            List<int> accessoryClothList = new List<int>();
            List<int> clothingClothList = new List<int>();

            foreach (KeyValuePair<ushort, Item> item in data.Items.Where(_ => noShopCloths.All(i => i != _.Value.ObjectId)))
            {
                if (item.Value.Texture1 != 0 && item.Value.ObjectId.Contains("Clothing") && item.Value.Class == "Dye")
                {
                    prices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(51, CurrencyType.Fame));
                    clothingDyeList.Add(item.Value.ObjectType);
                }

                if (item.Value.Texture2 != 0 && item.Value.ObjectId.Contains("Accessory") && item.Value.Class == "Dye")
                {
                    prices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(51, CurrencyType.Fame));
                    accessoryDyeList.Add(item.Value.ObjectType);
                }

                if (item.Value.Texture1 != 0 && item.Value.ObjectId.Contains("Cloth") &&
                    item.Value.ObjectId.Contains("Large"))
                {
                    prices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(160, CurrencyType.Fame));
                    clothingClothList.Add(item.Value.ObjectType);
                }

                if (item.Value.Texture2 != 0 && item.Value.ObjectId.Contains("Cloth") &&
                    item.Value.ObjectId.Contains("Small"))
                {
                    prices.Add(item.Value.ObjectType, new Tuple<int, CurrencyType>(160, CurrencyType.Fame));
                    accessoryClothList.Add(item.Value.ObjectType);
                }
            }

            ClothingDyeList = clothingDyeList.ToArray();
            ClothingClothList = clothingClothList.ToArray();
            AccessoryClothList = accessoryClothList.ToArray();
            AccessoryDyeList = accessoryDyeList.ToArray();
            log.Info("Merchat lists added.");
        }

        private static readonly string[] noShopCloths =
        {
            "Large Ivory Dragon Scale Cloth", "Small Ivory Dragon Scale Cloth",
            "Large Green Dragon Scale Cloth", "Small Green Dragon Scale Cloth",
            "Large Midnight Dragon Scale Cloth", "Small Midnight Dragon Scale Cloth",
            "Large Blue Dragon Scale Cloth", "Small Blue Dragon Scale Cloth",
            "Large Red Dragon Scale Cloth", "Small Red Dragon Scale Cloth",
            "Large Jester Argyle Cloth", "Small Jester Argyle Cloth",
            "Large Alchemist Cloth", "Small Alchemist Cloth",
            "Large Mosaic Cloth", "Small Mosaic Cloth",
            "Large Spooky Cloth", "Small Spooky Cloth",
            "Large Flame Cloth", "Small Flame Cloth",
            "Large Heavy Chainmail Cloth", "Small Heavy Chainmail Cloth",
        };
    }
}