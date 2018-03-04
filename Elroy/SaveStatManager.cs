using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elroy
{
    public static class SaveStatManager
    {
        internal static void UpdateSaveStatFromSave(string SaveFile)
        {
            using (var SaveStatReader = new BinaryReader(File.Open(SaveFile, FileMode.Open, FileAccess.Read)))
            {
                SaveStatReader.BaseStream.Position = 0x24;

                // "SAVESTAT_GAMES_CAMPAIGN"
                Form1.numericUpDown1.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_CAMPAIGN_NORMAL"
                Form1.numericUpDown2.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_CAMPAIGN_REVERSE"
                Form1.numericUpDown3.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_CHALLENGE"
                Form1.numericUpDown4.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER"
                Form1.numericUpDown5.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // ""SAVESTAT_GAMES_MULTIPLAYER_1V1"
                Form1.numericUpDown6.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_TAG"
                Form1.numericUpDown7.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_RANKED"
                Form1.numericUpDown8.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_FRIENDLY"
                Form1.numericUpDown9.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_ANY"
                Form1.numericUpDown10.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_1"
                Form1.numericUpDown11.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_2"
                Form1.numericUpDown12.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_3"
                Form1.numericUpDown13.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_CAMPAIGN"
                Form1.numericUpDown14.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_CAMPAIGN_NORMAL"
                Form1.numericUpDown15.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_CAMPAIGN_REVERSE"
                Form1.numericUpDown16.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_CHALLENGE"
                Form1.numericUpDown17.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER"
                Form1.numericUpDown18.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_1V1"
                Form1.numericUpDown19.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_TAG"
                Form1.numericUpDown20.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_RANKED"
                Form1.numericUpDown21.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_FRIENDLY"
                Form1.numericUpDown22.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_ANY"
                Form1.numericUpDown23.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_1"
                Form1.numericUpDown24.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_2"
                Form1.numericUpDown25.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_3"
                Form1.numericUpDown26.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_MATCH"
                Form1.numericUpDown27.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_WINS_NONMATCH"
                Form1.numericUpDown28.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_NORMAL"
                Form1.numericUpDown29.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_TRIBUTE"
                Form1.numericUpDown30.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_RITUAL"
                Form1.numericUpDown31.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_FUSION"
                Form1.numericUpDown32.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_XYZ"
                Form1.numericUpDown33.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_SYNCHRO"
                Form1.numericUpDown34.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_SUMMONS_PENDULUM"
                Form1.numericUpDown35.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_DAMAGE_ANY"
                Form1.numericUpDown36.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_DAMAGE_BATTLE"
                Form1.numericUpDown37.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // SAVESTAT_DAMAGE_DIRECT"
                Form1.numericUpDown38.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_DAMAGE_EFFECT"
                Form1.numericUpDown39.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_DAMAGE_REFLECT"
                Form1.numericUpDown40.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_CHAINS"
                Form1.numericUpDown41.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
                // "SAVESTAT_DECKS_CREATED"
                Form1.numericUpDown42.Value = BitConverter.ToInt32(SaveStatReader.ReadBytes(8), 0);
            }
        }

        internal static void WriteSaveStatToSave(string SaveFile)
        {
            using (var SaveStatWriter = new BinaryWriter(File.Open(SaveFile, FileMode.Open, FileAccess.Write)))
            {
                SaveStatWriter.BaseStream.Position = 0x24;

                // "SAVESTAT_GAMES_CAMPAIGN"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown1.Value));
                // "SAVESTAT_GAMES_CAMPAIGN_NORMAL"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown2.Value));
                // "SAVESTAT_GAMES_CAMPAIGN_REVERSE"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown3.Value));
                // "SAVESTAT_GAMES_CHALLENGE"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown4.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown5.Value));
                // ""SAVESTAT_GAMES_MULTIPLAYER_1V1"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown6.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_TAG"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown7.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_RANKED"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown8.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_FRIENDLY"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown9.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_ANY"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown10.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_1"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown11.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_2"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown12.Value));
                // "SAVESTAT_GAMES_MULTIPLAYER_BATTLEPACK_3"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown13.Value));
                // "SAVESTAT_WINS_CAMPAIGN"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown14.Value));
                // "SAVESTAT_WINS_CAMPAIGN_NORMAL"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown15.Value));
                // "SAVESTAT_WINS_CAMPAIGN_REVERSE"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown16.Value));
                // "SAVESTAT_WINS_CHALLENGE"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown17.Value));
                // "SAVESTAT_WINS_MULTIPLAYER"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown18.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_1V1"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown19.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_TAG"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown20.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_RANKED"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown21.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_FRIENDLY"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown22.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_ANY"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown23.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_1"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown24.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_2"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown25.Value));
                // "SAVESTAT_WINS_MULTIPLAYER_BATTLEPACK_3"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown26.Value));
                // "SAVESTAT_WINS_MATCH"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown27.Value));
                // "SAVESTAT_WINS_NONMATCH"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown28.Value));
                // "SAVESTAT_SUMMONS_NORMAL"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown29.Value));
                // "SAVESTAT_SUMMONS_TRIBUTE"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown30.Value));
                // "SAVESTAT_SUMMONS_RITUAL"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown31.Value));
                // "SAVESTAT_SUMMONS_FUSION"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown32.Value));
                // "SAVESTAT_SUMMONS_XYZ"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown33.Value));
                // "SAVESTAT_SUMMONS_SYNCHRO"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown34.Value));
                // "SAVESTAT_SUMMONS_PENDULUM"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown35.Value));
                // "SAVESTAT_DAMAGE_ANY"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown36.Value));
                // "SAVESTAT_DAMAGE_BATTLE"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown37.Value));
                // SAVESTAT_DAMAGE_DIRECT"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown38.Value));
                // "SAVESTAT_DAMAGE_EFFECT"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown39.Value));
                // "SAVESTAT_DAMAGE_REFLECT"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown40.Value));
                // "SAVESTAT_CHAINS"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown41.Value));
                // "SAVESTAT_DECKS_CREATED"
                SaveStatWriter.Write(BitConverter.GetBytes((long)Form1.numericUpDown42.Value));
            }
        }
    }
}
