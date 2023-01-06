using System;
using System.Collections.Generic;
using System.Drawing;

namespace SuperImageEvolver {
    public sealed class TaskStatistics {
        public TaskStatistics() {
            foreach (MutationType mutype in Enum.GetValues(typeof(MutationType))) {
                MutationCounts[mutype] = 0;
                MutationImprovements[mutype] = 0;
            }
        }

        public void Read(NBTag tag) {
            ImprovementCounter = tag.GetLong("ImprovementCounter", 0);
            MutationCounter = tag.GetLong("MutationCounter", 0);
            RiskyMoveCounter = tag.GetLong("RiskyMoveCounter", 0);
            FailedRiskCounter = tag.GetLong("FailedRiskCounter", 0);

            var statsTag = (NBTList)tag["MutationStats"];
            foreach (NBTag stat in statsTag) {
                MutationType mutationType = (MutationType)Enum.Parse(typeof(MutationType), stat["Type"].GetString());
                MutationCounts[mutationType] = stat["Count"].GetInt();
                MutationImprovements[mutationType] = stat["Sum"].GetDouble();
            }
        }

        public void Merge(NBTag tag) {
            MutationCounter += tag.GetLong("MutationCounter", 0);
            RiskyMoveCounter += tag.GetLong("RiskyMoveCounter", 0);
            FailedRiskCounter += tag.GetLong("FailedRiskCounter", 0);

            var statsTag = (NBTList)tag["MutationStats"];
            foreach (NBTag stat in statsTag) {
                MutationType mutationType = (MutationType)Enum.Parse(typeof(MutationType), stat["Type"].GetString());
                MutationCounts[mutationType] += stat["Count"].GetInt();
                MutationImprovements[mutationType] += stat["Sum"].GetDouble();
            }
        }

        public void Store(NBTag tag) {
            tag.Append("ImprovementCounter", ImprovementCounter);
            tag.Append("MutationCounter", MutationCounter);
            tag.Append("RiskyMoveCounter", RiskyMoveCounter);
            tag.Append("FailedRiskCounter", FailedRiskCounter);

            List<NBTCompound> statTags = new List<NBTCompound>();
            foreach (MutationType mtype in Enum.GetValues(typeof(MutationType))) {
                NBTCompound stat = new NBTCompound("MutationTypeStat");
                stat.Append("Type", mtype.ToString());
                stat.Append("Count", MutationCounts[mtype]);
                stat.Append("Sum", MutationImprovements[mtype]);
                statTags.Add(stat);
            }
            var stats = new NBTList("MutationStats", NBTType.Compound, statTags.ToArray());
            tag.Append(stats);
        }

        public void Reset() {
            ImprovementCounter = 0;
            MutationCounter = 0;
            RiskyMoveCounter = 0;
            FailedRiskCounter = 0;
            MutationDataLog.Clear();
            foreach (MutationType type in Enum.GetValues(typeof(MutationType))) {
                MutationCounts[type] = 0;
                MutationImprovements[type] = 0;
            }
        }

        public long ImprovementCounter, MutationCounter, RiskyMoveCounter, FailedRiskCounter;
        public readonly List<PointF> MutationDataLog = new List<PointF>();
        public readonly Dictionary<MutationType, int> MutationCounts = new Dictionary<MutationType, int>();
        public readonly Dictionary<MutationType, double> MutationImprovements = new Dictionary<MutationType, double>();
    }
}
