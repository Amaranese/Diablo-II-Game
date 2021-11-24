﻿using System.Collections.Generic;
using Diablerie.Engine.IO.D2Formats;
using Diablerie.Engine.LibraryExtensions;

namespace Diablerie.Engine.Datasheets
{
    [System.Serializable]
    [Datasheet.Record]
    public class ItemType
    {
        public static List<ItemType> sheet;
        static Dictionary<string, ItemType> byCode = new Dictionary<string, ItemType>();

        public static ItemType Find(string code)
        {
            return byCode.GetValueOrDefault(code);
        }

        public static void Load()
        {
            sheet = Datasheet.Load<ItemType>("data/global/excel/ItemTypes.txt");
            foreach (ItemType type in sheet)
            {
                if (type.code == null)
                    continue;
                if (byCode.ContainsKey(type.code))
                    continue;
                if (type.body)
                {
                    type.bodyLoc1 = BodyLoc.GetIndex(type.bodyLoc1Code);
                    type.bodyLoc2 = BodyLoc.GetIndex(type.bodyLoc2Code);
                }
                byCode.Add(type.code, type);
            }

            foreach (ItemType type in sheet)
            {
                if (type._equiv1 != null)
                    type.equiv1 = Find(type._equiv1);
                if (type._equiv2 != null)
                    type.equiv2 = Find(type._equiv2);
            }
        }

        public bool Is(ItemType type)
        {
            if (this == type)
                return true;

            return (equiv1 != null && equiv1.Is(type)) || (equiv2 != null && equiv2.Is(type));
        }

        public string name;
        public string code;
        public string _equiv1;
        public string _equiv2;
        public bool repair;
        public bool body;
        public string bodyLoc1Code;
        public string bodyLoc2Code;
        public string shoots;
        public string quiver;
        public bool throwable;
        public bool reload;
        public bool reEquip;
        public bool autoStack;
        public bool alwaysMagic;
        public bool canBeRare;
        public bool alwaysNormal;
        public bool charm;
        public bool gem;
        public bool beltable;
        public int maxSock1;
        public int maxSock25;
        public int maxSock40;
        public bool treasureClass;
        public int rarity;
        public string staffMods;
        public string costFormula;
        public string classCode;
        public int varInvGfx;
        [Datasheet.Sequence(length = 6)]
        public string[] invGfx;
        public string storePage;
        public string eol;

        [System.NonSerialized]
        public ItemType equiv1;

        [System.NonSerialized]
        public ItemType equiv2;

        [System.NonSerialized]
        public int bodyLoc1;

        [System.NonSerialized]
        public int bodyLoc2;
    }
}
