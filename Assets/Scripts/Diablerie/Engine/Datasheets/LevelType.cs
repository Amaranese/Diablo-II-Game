﻿using System.Collections.Generic;
using Diablerie.Engine.IO.D2Formats;

namespace Diablerie.Engine.Datasheets
{
    [System.Serializable]
    [Datasheet.Record]
    public class LevelType
    {
        public string name;
        public int id;
        [Datasheet.Sequence(length = 32)]
        public string[] files;
        public bool beta;
        public int act;

        [System.NonSerialized]
        public List<string> dt1Files = new List<string>();

        public static List<LevelType> sheet;

        public static void Load()
        {
            sheet = Datasheet.Load<LevelType>("data/global/excel/LvlTypes.txt");
            foreach (var levelType in sheet)
            {
                foreach (var file in levelType.files)
                {
                    if (file == "0" || file == null)
                        continue;

                    levelType.dt1Files.Add(@"data\global\tiles\" + file.Replace("/", @"\"));
                }
            }
        }
    }
}
