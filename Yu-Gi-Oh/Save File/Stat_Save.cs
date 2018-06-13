using System;
using System.Collections.Generic;
using System.IO;

namespace Yu_Gi_Oh.Save_File
{
    public class Stat_Save : Save_Data_Chunk
    {
        private const int NumberOfSaveStats = 43;

        public Stat_Save()
        {
            Stats = new Dictionary<StatSaveType, long>();
        }

        /// <summary>
        ///     Public dictionary of SaveStats and their values.
        /// </summary>
        public Dictionary<StatSaveType, long> Stats { get; }

        /// <summary>
        ///     Clear all SaveStat values back to 0.
        /// </summary>
        public override void Clear()
        {
            foreach (StatSaveType stat in Enum.GetValues(typeof(StatSaveType))) Stats[stat] = 0;
        }

        /// <summary>
        ///     Loads SaveStat values into the Stats Dictionary
        /// </summary>
        /// <seealso cref="Stats" />
        /// <param name="reader">An instance of BinaryReader used for loading the save file.</param>
        public override void Load(BinaryReader reader)
        {
            for (var count = 0; count < NumberOfSaveStats; count++) Stats[(StatSaveType) count] = reader.ReadInt64();
        }

        /// <summary>
        ///     Saves SaveStat values from the Stats Dictionary back to the Save File
        /// </summary>
        /// <seealso cref="Stats" />
        /// <param name="writer">An instance of BinaryWriter for saving to the save file.</param>
        public override void Save(BinaryWriter writer)
        {
            for (var i = 0; i < NumberOfSaveStats; i++)
            {
                Stats.TryGetValue((StatSaveType) i, out var value);
                writer.Write(value);
            }
        }
    }
}