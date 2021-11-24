﻿using System.Collections.Generic;
using Diablerie.Engine.IO.D2Formats;
using Diablerie.Engine.LibraryExtensions;

namespace Diablerie.Engine.Datasheets
{
    [System.Serializable]
    [Datasheet.Record]
    public class Translation
    {
        public string key;
        public string value;

        static Dictionary<string, string> map = new Dictionary<string, string>();
        public static List<Translation> sheet;
        public static List<Translation> expansionSheet;
        public static List<Translation> patchSheet;

        public static void Load()
        {
            sheet = Datasheet.Load<Translation>("data/local/string.txt", headerLines: 0);
            expansionSheet = Datasheet.Load<Translation>("data/local/expansionstring.txt", headerLines: 0);
            patchSheet = Datasheet.Load<Translation>("data/local/patchstring.txt", headerLines: 0);
            foreach (var translation in patchSheet)
            {
                if (!map.ContainsKey(translation.key))
                    map.Add(translation.key, translation.value);
            }

            foreach (var translation in expansionSheet)
            {
                if (!map.ContainsKey(translation.key))
                    map.Add(translation.key, translation.value);
            }

            foreach (var translation in sheet)
            {
                if (!map.ContainsKey(translation.key))
                    map.Add(translation.key, translation.value);
            }
        }

        public static string Find(string key)
        {
            return Find(key, key);
        }

        public static string Find(string key, string defaultValue = null)
        {
            if (key == null)
                return null;
            return map.GetValueOrDefault(key, defaultValue);
        }
    }
}
