using System.Collections.Generic;
using System.IO;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public class Campaign_Save : Save_Data_Chunk
    {
        public const int DuelsPerSeries = 50;

        public Campaign_Save()
        {
            DuelsBySeries = new Dictionary<Duel_Series, Duel[]>
            {
                {Duel_Series.YuGiOh, new Duel[DuelsPerSeries]},
                {Duel_Series.YuGiOhGX, new Duel[DuelsPerSeries]},
                {Duel_Series.YuGiOh5D, new Duel[DuelsPerSeries]},
                {Duel_Series.YuGiOhZEXAL, new Duel[DuelsPerSeries]},
                {Duel_Series.YuGiOhARCV, new Duel[DuelsPerSeries]}
            };

            foreach (var seriesDuels in DuelsBySeries)
                for (var i = 0; i < DuelsPerSeries; i++)
                    seriesDuels.Value[i] = new Duel();
        }

        public Dictionary<Duel_Series, Duel[]> DuelsBySeries { get; }

        public override void Clear()
        {
            foreach (var seriesDuels in DuelsBySeries)
                for (var i = 0; i < DuelsPerSeries; i++)
                {
                    var duel = seriesDuels.Value[i];
                    duel.State = i == 0 ? CampaignDuelState.Available : CampaignDuelState.Locked;
                    duel.ReverseDuelState = CampaignDuelState.Locked;
                    duel.Unk1 = 0;
                    duel.Unk2 = 0;
                    duel.Unk3 = 0;
                    duel.Unk4 = 0;
                }
        }

        public override void Load(BinaryReader reader)
        {
            reader.ReadInt32();
            reader.ReadInt32();

            for (var i = 0; i < Constants.NumDuelSeries; i++)
            {
                var series = IndexToSeries(i);

                var duels = DuelsBySeries[series];
                for (var j = 0; j < DuelsPerSeries; j++)
                {
                    duels[j].Read(reader);
                    if (j != 0) continue;
                    reader.ReadInt32();
                    reader.ReadInt32();
                }
            }
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(0);
            writer.Write(1);

            for (var i = 0; i < Constants.NumDuelSeries; i++)
            {
                var series = IndexToSeries(i);

                DuelsBySeries.TryGetValue(series, out var duels);

                for (var j = 0; j < DuelsPerSeries; j++)
                {
                    duels?[j].Write(writer);
                    if (j != 0) continue;
                    writer.Write((uint) 0);
                    writer.Write((uint) 0);
                }
            }
        }

        private int SeriesToIndex(Duel_Series series)
        {
            return (int) series;
        }

        private static Duel_Series IndexToSeries(int index)
        {
            return (Duel_Series) index;
        }

        public class Duel
        {
            public CampaignDuelState State { get; set; }
            public CampaignDuelState ReverseDuelState { get; set; }
            public int Unk1 { get; set; }
            public int Unk2 { get; set; }
            public int Unk3 { get; set; }
            public int Unk4 { get; set; }

            public void Read(BinaryReader reader)
            {
                State = (CampaignDuelState) reader.ReadInt32();
                ReverseDuelState = (CampaignDuelState) reader.ReadInt32();
                Unk1 = reader.ReadInt32();
                Unk2 = reader.ReadInt32();
                Unk3 = reader.ReadInt32();
                Unk4 = reader.ReadInt32();
            }

            public void Write(BinaryWriter writer)
            {
                writer.Write((int) State);
                writer.Write((int) ReverseDuelState);
                writer.Write(Unk1);
                writer.Write(Unk2);
                writer.Write(Unk3);
                writer.Write(Unk4);
            }
        }
    }
}