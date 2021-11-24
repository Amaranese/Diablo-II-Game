﻿using Diablerie.Engine.Datasheets;
using Diablerie.Engine.Entities;
using Diablerie.Engine.IO.D2Formats;
using UnityEngine;

namespace Diablerie.Engine
{
    public class Equipment : MonoBehaviour
    {
        public Item[] items;

        public delegate void OnUpdateHandler();
        public event OnUpdateHandler OnUpdate;

        public CharStatsInfo charInfo;
        private Unit _unit;
        private COFRenderer _renderer;

        static string[] armorTypes = new string[] { "LIT", "MED", "HVY" };
        static string[] defaultEquip = new string[] { "LIT", "LIT", "LIT", "LIT", "LIT", "", "", "", "LIT", "LIT", "", "", "", "", "", "" };
        static Item[] unequippedItems = new Item[2];

        public bool CanEquip(Item item)
        {
            if (!item.identified)
                return false;

            if (!item.info.type.body)
                return false;

            var classCode = item.info.type.classCode;
            if (classCode != null && charInfo.code != classCode)
                return false;

            return true;
        }

        public bool CanEquip(Item item, int loc)
        {
            if (!CanEquip(item))
                return false;

            if (loc != item.info.type.bodyLoc1 && loc != item.info.type.bodyLoc2)
                return false;

            return true;
        }

        public bool Equip(Item item)
        {
            if (!CanEquip(item))
                return false;

            if (items[item.info.type.bodyLoc1] != null || items[item.info.type.bodyLoc2] != null)
                return false;
            Equip(item, item.info.type.bodyLoc1);

            if (OnUpdate != null)
                OnUpdate();

            return true;
        }

        public Item[] Equip(Item item, int loc)
        {
            unequippedItems[0] = null;
            unequippedItems[1] = null;

            if (item != null && !CanEquip(item, loc))
                return unequippedItems;

            if (items[loc] != null)
                unequippedItems[0] = items[loc];

            if (item != null)
            {
                int loc2 = loc == item.info.type.bodyLoc1 ? item.info.type.bodyLoc2 : item.info.type.bodyLoc1;
                if (items[loc2] != null)
                {
                    bool pop = item.info.weapon != null && (items[loc2].info.weapon != null || item.info.weapon.twoHanded);
                    if (pop)
                    {
                        unequippedItems[1] = items[loc2];
                        items[loc2] = null;
                    }
                }
            }

            items[loc] = item;

            UpdateAnimator();

            if (OnUpdate != null)
                OnUpdate();

            return unequippedItems;
        }

        public Item GetWeapon()
        {
            foreach(var item in items)
            {
                if (item != null && item.info.weapon != null)
                    return item;
            }

            return null;
        }

        public bool IsEquipped(ItemInfo itemInfo)
        {
            Item item1 = items[itemInfo.type.bodyLoc1];
            if (item1 != null && item1.info == itemInfo)
                return true;

            Item item2 = items[itemInfo.type.bodyLoc2];
            if (item2 != null && item2.info == itemInfo)
                return true;

            return false;
        }

        void UpdateAnimator()
        {
            _unit.weaponClass = "HTH";
            var equip = _renderer.equip;
            if (equip == null)
                equip = new string[defaultEquip.Length];
            System.Array.Copy(defaultEquip, equip, defaultEquip.Length);
            foreach(var item in items)
            {
                if (item == null)
                    continue;
                if (item.info.armor != null && item.info.armor.rArm != -1)
                {
                    equip[System.Array.IndexOf(COF.layerNames, "RA")] = armorTypes[item.info.armor.rArm];
                    equip[System.Array.IndexOf(COF.layerNames, "LA")] = armorTypes[item.info.armor.lArm];
                    equip[System.Array.IndexOf(COF.layerNames, "TR")] = armorTypes[item.info.armor.torso];
                    equip[System.Array.IndexOf(COF.layerNames, "LG")] = armorTypes[item.info.armor.legs];
                    equip[System.Array.IndexOf(COF.layerNames, "S1")] = armorTypes[item.info.armor.rSPad];
                    equip[System.Array.IndexOf(COF.layerNames, "S2")] = armorTypes[item.info.armor.lSPad];
                }
                else
                {
                    if (item.info.component >= 0 && item.info.component < equip.Length)
                        equip[item.info.component] = item.info.alternateGfx;
                    if (item.info.weapon != null)
                    {
                        if (item.info.weapon.twoHanded)
                            _unit.weaponClass = item.info.weapon.twoHandedWClass;
                        else
                            _unit.weaponClass = item.info.weapon.wClass;
                    }
                }
            }
            _renderer.equip = equip;
        }

        void Awake()
        {
            _unit = GetComponent<Unit>();
            _renderer = GetComponent<COFRenderer>();
            items = new Item[BodyLoc.sheet.Count];
        }

        private void Start()
        {
            UpdateAnimator();
        }
    }
}
