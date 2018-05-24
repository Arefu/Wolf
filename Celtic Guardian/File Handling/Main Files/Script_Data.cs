using Celtic_Guardian.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Celtic_Guardian.Main_Files
{
    public class Script_Data : File_Data
    {
        static Encoding encoding = Encoding.UTF8;
        public List<Script> Scripts { get; private set; }
        public List<ScriptText> Text { get; private set; }

        public override bool IsLocalized
        {
            get { return true; }
        }

        public Script_Data()
        {
            Scripts = new List<Script>();
            Text = new List<ScriptText>();
        }

        public override void Load(BinaryReader reader, long length, Localized_Text.Language language)
        {
            long fileStartPos = reader.BaseStream.Position;

            long firstChunkSize = reader.ReadInt64();

            int firstChunkItemCount = reader.ReadInt32();
            int secondChunkItemCount = reader.ReadInt32();

            for (int i = 0; i < firstChunkItemCount; i++)
            {
                int textStartIndex = reader.ReadInt32();
                int textEndIndex = reader.ReadInt32();
                long codeNameOffset = reader.ReadInt64();

                long tempOffset = reader.BaseStream.Position;
                reader.BaseStream.Position = fileStartPos + codeNameOffset;
                string codeName = reader.ReadNullTerminatedString(encoding);// Is this ascii or utf-8?
                reader.BaseStream.Position = tempOffset;

                Script script = null;
                if (Scripts.Count > i)
                {
                    script = Scripts[i];
                }
                else
                {
                    Scripts.Add(script = new Script(textStartIndex, textEndIndex));
                }
                script.CodeName.SetText(language, codeName);
            }

            reader.BaseStream.Position = fileStartPos + firstChunkSize;

            for (int i = 0; i < secondChunkItemCount; i++)
            {
                long characterNameOffset = reader.ReadInt64();
                long characterPositionOffset = reader.ReadInt64();
                long characterExpressionOffset = reader.ReadInt64();
                long textOffset = reader.ReadInt64();

                long tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + characterNameOffset;
                string characterName = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = fileStartPos + characterPositionOffset;
                string characterPosition = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = fileStartPos + characterExpressionOffset;
                string characterExpression = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = fileStartPos + textOffset;
                string text = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = tempOffset;

                ScriptText scriptText = null;
                if (Text.Count > i)
                {
                    scriptText = Text[i];
                }
                else
                {
                    Text.Add(scriptText = new ScriptText());
                }
                scriptText.CharacterName.SetText(language, characterName);
                scriptText.CharacterPosition.SetText(language, characterPosition);
                scriptText.CharacterExpression.SetText(language, characterExpression);
                scriptText.Text.SetText(language, text);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            int firstChunkOffsetsItemSize = 4 + 4 + 8;// text start + text end + string offset
            int secondChunkOffsetsItemSize = 8 * 4;// 4 strings

            // Save the file start offset as we will be writing the first chunk size once we know it
            long fileStartPos = writer.BaseStream.Position;
            writer.Write((long)0);

            writer.Write(Scripts.Count);
            writer.Write(Text.Count);

            long firstChunkOffsetsOffset = writer.BaseStream.Position;
            long firstChunkOffsetsSize = Scripts.Count * firstChunkOffsetsItemSize;
            writer.Write(new byte[firstChunkOffsetsSize]);

            for (int i = 0; i < Scripts.Count; i++)
            {
                Script script = Scripts[i];

                long tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = firstChunkOffsetsOffset + (i * firstChunkOffsetsItemSize);
                writer.Write(script.TextStartIndex);
                writer.Write(script.TextEndIndex);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(script.CodeName.GetText(language), encoding);// Is this ascii or utf-8?
            }

            // We now know the size of the first chunk. Write it to the buffer.
            long temp = writer.BaseStream.Position;
            writer.BaseStream.Position = fileStartPos;
            writer.Write(temp - fileStartPos);
            writer.BaseStream.Position = temp;

            long secondChunkOffsetsOffset = writer.BaseStream.Position;
            long secondChunkOffsetsSize = Text.Count * secondChunkOffsetsItemSize;
            writer.Write(new byte[secondChunkOffsetsSize]);

            for (int i = 0; i < Text.Count; i++)
            {
                ScriptText scriptText = Text[i];

                int characterNameLen = GetStringSize(scriptText.CharacterName.GetText(language), encoding);
                int characterPositionLen = GetStringSize(scriptText.CharacterPosition.GetText(language), encoding);
                int characterExpressionLen = GetStringSize(scriptText.CharacterExpression.GetText(language), encoding);
                long tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = secondChunkOffsetsOffset + (i * secondChunkOffsetsItemSize);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + characterNameLen);
                writer.WriteOffset(fileStartPos, tempOffset + characterNameLen + characterPositionLen);
                writer.WriteOffset(fileStartPos, tempOffset + characterNameLen + characterPositionLen + characterExpressionLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(scriptText.CharacterName.GetText(language), encoding);
                writer.WriteNullTerminatedString(scriptText.CharacterPosition.GetText(language), encoding);
                writer.WriteNullTerminatedString(scriptText.CharacterExpression.GetText(language), encoding);
                writer.WriteNullTerminatedString(scriptText.Text.GetText(language), encoding);
            }
        }

        public override void Clear()
        {
            Scripts.Clear();
            Text.Clear();
        }

        public class Script
        {
            /// <summary>
            /// The code name for this script. e.g. "TheDuelistKingdom_INTRO" "TheDuelistKingdom_OUTRO"
            /// </summary>
            public Localized_Text CodeName { get; set; }

            /// <summary>
            /// The start index of this script. This is an index into the script text collection.
            /// This script consists of all script text between start index / end index.
            /// </summary>
            public int TextStartIndex { get; set; }

            /// <summary>
            /// The end index of this script. This is an index into the script text collection.
            /// This script consists of all script text between start index / end index.
            /// </summary>
            public int TextEndIndex { get; set; }

            public Script(int textStartIndex, int textEndIndex)
            {
                TextStartIndex = textStartIndex;
                TextEndIndex = textEndIndex;
                CodeName = new LocalizedText();
            }

            public override string ToString()
            {
                return "codeName: '" + CodeName + "' textStartIndex: " + TextEndIndex + " textEndIndex: " + TextEndIndex;
            }
        }

        public class ScriptText
        {
            /// <summary>
            /// Character name. e.g. "infn8", "tristantaylor", "joeywheeler", "yugimuto"
            /// </summary>
            public Localized_Text CharacterName { get; set; }

            /// <summary>
            /// Where the character is positioned on screen. e.g. "RIGHT", "NONE", "RIGHT_BACK", "CENTER", "LEFT:FADEIN", "FADEOUT".
            /// "NONE" removes the character. An empty string is assumed to be default position / no position change if already existing.
            /// </summary>
            public Localized_Text CharacterPosition { get; set; }

            /// <summary>
            /// The characters facial expression. e.g. "neutral", "anger", "sad"
            /// An empty string will show the default facial expression.
            /// </summary>
            public Localized_Text CharacterExpression { get; set; }

            /// <summary>
            /// The text that is displayed in the script box.
            /// (if left blank no text will be displayed but the character will update its position / expression)
            /// </summary>
            public Localized_Text Text { get; set; }

            public ScriptText()
            {
                CharacterName = new Localized_Text();
                CharacterPosition = new Localized_Text();
                CharacterExpression = new Localized_Text();
                Text = new Localized_Text();
            }

            public override string ToString()
            {
                return "characterName: '" + CharacterName + "' characterPosition: '" + CharacterPosition +
                    "' characterExpression: '" + CharacterExpression + "' text: '" + Text + "'";
            }
        }
    }
}