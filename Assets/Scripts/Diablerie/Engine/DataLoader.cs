using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Diablerie.Engine.Datasheets;
using Diablerie.Engine.IO.D2Formats;
using Debug = UnityEngine.Debug;

namespace Diablerie.Engine
{
    public class DataLoader
    {
        public struct Paths
        {
            public MpqLocation[] mpq;
            public string animData;
        }

        public struct MpqLocation
        {
            public string filename;
            public bool optional;
        }
        
        public class LoadProgress
        {
            public int totalCount;
            public int doneCount;
            public bool finished;
            public Exception exception;
        }

        private Paths paths;

        public DataLoader(Paths paths)
        {
            this.paths = paths;
        }

        public LoadProgress LoadAll()
        {
            var progress = new LoadProgress();
            Task.Run(() => LoadAll(progress));
            return progress;
        }

        private void LoadAll(LoadProgress progress)
        {
            var sw = Stopwatch.StartNew();
            List<Action> actions = new List<Action>();
            foreach (var mpqLocation in paths.mpq)
            {
                actions.Add(() => Mpq.AddArchive(mpqLocation.filename, mpqLocation.optional));
            }
            actions.Add(() => AnimData.Load(paths.animData));
            actions.Add(Translation.Load);
            actions.Add(SoundInfo.Load);
            actions.Add(SoundEnvironment.Load);
            actions.Add(ObjectInfo.Load);
            actions.Add(BodyLoc.Load);
            actions.Add(ExpTable.Load);
            actions.Add(LevelType.Load);
            actions.Add(LevelWarpInfo.Load);
            actions.Add(LevelPreset.Load);
            actions.Add(LevelMazeInfo.Load);
            actions.Add(LevelInfo.Load);
            actions.Add(OverlayInfo.Load);
            actions.Add(MissileInfo.Load);
            actions.Add(ItemStat.Load);
            actions.Add(ItemRatio.Load);
            actions.Add(ItemType.Load);
            actions.Add(ItemPropertyInfo.Load);
            actions.Add(ItemSet.Load);
            actions.Add(UniqueItem.Load);
            actions.Add(SetItem.Load);
            actions.Add(TreasureClass.Load);
            actions.Add(MagicAffix.Load);
            actions.Add(CharStatsInfo.Load);
            actions.Add(MonLvl.Load);
            actions.Add(MonPreset.Load);
            actions.Add(MonSound.Load);
            actions.Add(MonStatsExtended.Load);
            actions.Add(MonStat.Load);
            actions.Add(SuperUnique.Load);
            actions.Add(SkillDescription.Load);
            actions.Add(SkillInfo.Load);
            actions.Add(SpawnPreset.Load);
            actions.Add(StateInfo.Load);
            progress.totalCount = actions.Count;
            try
            {
                foreach (Action action in actions)
                {
                    action();
                    progress.doneCount++;
                }
            }
            catch (Exception e)
            {
                progress.exception = e;
            }
            progress.finished = true;
            Debug.Log("DataLoader finished in " + sw.ElapsedMilliseconds + " ms");
        }
    }
}