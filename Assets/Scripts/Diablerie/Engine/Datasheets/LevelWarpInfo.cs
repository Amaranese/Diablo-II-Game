﻿using System.Collections.Generic;
using Diablerie.Engine.IO.D2Formats;
using Diablerie.Engine.World;

namespace Diablerie.Engine.Datasheets
{
    [System.Serializable]
    [Datasheet.Record]
    public class LevelWarpInfo
    {
        public string name;
        public int id;
        public int selectX;
        public int selectY;
        public int selectDX;
        public int selectDY;
        public int exitWalkX;
        public int exitWalkY;
        public int offsetX;
        public int offsetY;
        public int litVersion;
        public int tiles;
        public string direction;
        public int beta;

        [System.NonSerialized]
        public Warp instance;

        public static List<LevelWarpInfo> sheet;
        static Dictionary<int, LevelWarpInfo> idMap = new Dictionary<int, LevelWarpInfo>();

        public static void Load()
        {
            sheet = Datasheet.Load<LevelWarpInfo>("data/global/excel/LvlWarp.txt");
            foreach(var warpInfo in sheet)
            {
                idMap[warpInfo.id] = warpInfo;
            }
        }

        public static LevelWarpInfo Find(int id)
        {
            return idMap[id];
        }
    }
}
