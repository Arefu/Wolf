using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Elroy
{
    internal static class StoryManager
    {
        internal static void UpdateCampaignFromSave(ref TabPage Page, string SaveName)
        {
            var SkipOffset = new List<long>
            {
                //Yu-Gi-Oh!
                0x16F8, /* DLC */
                0x1740, /* DLC */
                0x18A8, /* Blank */
                0x18C0, /* Blank */
                0x18D8, /* Blank */
                0x18F0, /* Blank */
                0x1908, /* Blank */

                /*Yu-Gi-Oh! GX*/
                0x1C40, /* DLC */
                0x1C70, /* DLC */

                /*Yu-Gi-Oh! 5D's*/
                //Didn't See Any For This Game, If You Notice Any Please Let Me Know In Discord or Github Issues.

                /*Yu-Gi-Oh! ZEXAL*/
                0x24C0, /* DLC */
                0x2658 /* DLC */


                /*Yu-Gi-Oh! ARC-V*/
                //All Levels For This Arc Are DLC.
            };

            var StoryLengths = new List<StoryLookup>
            {
                /*Yu-Gi-Oh!*/
                new StoryLookup(0x1650, 0x1920, 23, 1),
                /*Yu-Gi-Oh! GX*/
                new StoryLookup(0x1B08, 0x1DC0, 26, 2),
                /*Yu-Gi-Oh! 5D's*/
                new StoryLookup(0x1FC0, 0x2260, 27, 3),
                /*Yu-Gi-Oh! ZEXAL*/
                new StoryLookup(0x2460, 0x26D0, 22, 4)
            };

            using (var CampaignReader = new BinaryReader(File.Open(SaveName, FileMode.Open, FileAccess.Read)))
            {
                var CurrentStory = 0;
                foreach (var GroupBox in Page.Controls.OfType<GroupBox>().Reverse())
                foreach (var CheckBoxList in GroupBox.Controls.OfType<CheckedListBox>())
                {
                    CampaignReader.BaseStream.Position = StoryLengths[CurrentStory].Start;
                    var CurrentLevel = 0;
                    do
                    {
                        if (CurrentLevel > StoryLengths[CurrentStory].Missions) break;
                        if (SkipOffset.Any(Offset => CampaignReader.BaseStream.Position == Offset))
                        {
                            CampaignReader.BaseStream.Position += 0x18;
                            continue;
                        }

                        CheckBoxList.SetItemChecked(CurrentLevel, ValidateStory(CampaignReader.ReadBytes(0x18), CampaignReader.BaseStream.Position, CurrentLevel));


                        CurrentLevel++;
                    } while (CampaignReader.BaseStream.Position <= StoryLengths[CurrentStory].End);

                    CurrentStory++;
                }
            }
        }

        internal static bool ValidateStory(byte[] DataChunk, long CurrentOffset, int CurrentLevel)
        {
            return DataChunk[0] == 0x03 || DataChunk[0] == 0x01;
        }

        internal static void WriteCampaignToSave()
        {
        }
    }

    internal class StoryLookup
    {
        //May be bale to remove missions
        public StoryLookup(long Start, long End, int Missions, int Story)
        {
            this.Start = Start;
            this.End = End;
            this.Story = Story;
            this.Missions = Missions;
        }

        public long Start { get; set; }
        public long End { get; set; }
        public int Story { get; set; }
        public int Missions { get; set; }
    }
}