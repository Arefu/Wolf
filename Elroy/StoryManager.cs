using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Elroy
{
    internal static class StoryManager
    {
        internal static List<long> SkipOffset = new List<long>
        {
            //Yu-Gi-Oh!
            0x16F8, /* DLC */
            0x1740, /* DLC */
            0x18A8, /* Blank? */
            0x18C0, /* Blank? */
            0x18D8, /* Blank? */
            0x18F0, /* Blank? */
            0x1908, /* Blank? */

            /*Yu-Gi-Oh! GX*/
            0x1C40, /* DLC */
            0x1C70, /* DLC */

            /*Yu-Gi-Oh! 5D's*/
            //I Didn't See Any For This Game, If You Notice Any Please Let Me Know In Discord or Github Issues.

            /*Yu-Gi-Oh! ZEXAL*/
            0x24C0, /* DLC */
            0x2658 /* DLC */
        };

        internal static List<StoryLookup> StoryLengths = new List<StoryLookup>
        {
            /*Yu-Gi-Oh!*/
            new StoryLookup(0x1650, 0x1920, 23),
            /*Yu-Gi-Oh! GX*/
            new StoryLookup(0x1B08, 0x1DC0, 26),
            /*Yu-Gi-Oh! 5D's*/
            new StoryLookup(0x1FC0, 0x2260, 27),
            /*Yu-Gi-Oh! ZEXAL*/
            new StoryLookup(0x2478, 0x26D0, 22)
        };

        internal static void UpdateCampaignFromSave(ref TabPage Page, string SaveName)
        {
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

                        CheckBoxList.SetItemChecked(CurrentLevel, ValidateStory(CampaignReader.ReadBytes(0x18)));


                        CurrentLevel++;
                    } while (CampaignReader.BaseStream.Position <= StoryLengths[CurrentStory].End);

                    CurrentStory++;
                }
            }
        }

        internal static void WriteCampaignToSave(ref TabPage Page, string SaveName)
        {
            using (var CampaignWriter = new BinaryWriter(File.Open(SaveName, FileMode.Open, FileAccess.Write)))
            {
                var CurrentStory = 0;
                foreach (var GroupBox in Page.Controls.OfType<GroupBox>().Reverse())
                foreach (var CheckBoxList in GroupBox.Controls.OfType<CheckedListBox>())
                {
                    CampaignWriter.BaseStream.Position = StoryLengths[CurrentStory].Start;
                    var CurrentLevel = 0;
                    do
                    {
                        if (CurrentLevel > StoryLengths[CurrentStory].Missions) break;
                        if (SkipOffset.Any(Offset => CampaignWriter.BaseStream.Position == Offset))
                        {
                            CampaignWriter.BaseStream.Position += 0x18;
                            continue;
                        }

                        if (CheckBoxList.GetItemChecked(CurrentLevel))
                        {
                            CampaignWriter.Write(new byte[] {0x3, 0x0, 0x0, 0x0, 0x3});
                            CampaignWriter.Write(new byte[19]);
                        }
                        else
                        {
                            CampaignWriter.Write(new byte[24]);
                        }


                        CurrentLevel++;
                    } while (CampaignWriter.BaseStream.Position <= StoryLengths[CurrentStory].End);

                    CurrentStory++;
                }
            }
        }


        internal static bool ValidateStory(byte[] DataChunk)
        {
            return DataChunk[0] == 0x01 || DataChunk[0] == 0x02 || DataChunk[0] == 0x03; //0x01 - Not Yet Played, 0x02 - Draw, 0x03 - Win, 0x00 - Default / Locked.
        }
    }
}