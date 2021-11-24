﻿using Diablerie.Engine.IO.D2Formats;

namespace Diablerie.Engine.Datasheets
{
    [System.Serializable]
    public class ArmorInfo : ItemInfo
    {
        public string _name;
        public string version;
        public string compactSave;
        public int rarity;
        public bool spawnable;
        public int minAC;
        public int maxAC;
        public int absorbs;
        public int speed;
        public int reqStr;
        public int block;
        public int durability;
        public bool noDurability;
        public int _level;
        public int _levelReq;
        public int _cost;
        public int _gambleCost;
        public string _code;
        public string nameStr;
        public int magicLvl;
        public string autoPrefix;
        public string _alternateGfx;
        public string openBetaGfx;
        public string _normCode;
        public string _uberCode;
        public string _ultraCode;
        public int spellOffset;
        public int _component;
        public int _invWidth;
        public int _invHeight;
        public bool hasInv;
        public int gemSockets;
        public string gemApplyType;
        public string _flippyFile;
        public string _invFile;
        public string _uniqueInvFile;
        public string _setInvFile;
        public int rArm = -1;
        public int lArm = -1;
        public int torso = -1;
        public int legs = -1;
        public int rSPad = -1;
        public int lSPad = -1;
        public bool usable;
        public bool throwable;
        public bool stackable;
        public int minStack;
        public int maxStack;
        public string _type1;
        public string _type2;
        public string _dropSound;
        public int _dropSoundFrame;
        public string _useSound;
        public bool _alwaysUnique;
        [Datasheet.Sequence(length = 110)]
        public string[] skipped2;
    }
}
