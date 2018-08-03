using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class Script_Data : File_Data
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public Script_Data()
        {
            Scripts = new List<Script>();
            Text = new List<ScriptText>();
        }

        public List<Script> Scripts { get; }
        public List<ScriptText> Text { get; }

        public override bool IsLocalized => true;

        public override void Load(BinaryReader reader, long length, Localized_Text.Language language)
        {
            var fileStartPos = reader.BaseStream.Position;

            var firstChunkSize = reader.ReadInt64();

            var firstChunkItemCount = reader.ReadInt32();
            var secondChunkItemCount = reader.ReadInt32();

            for (var i = 0; i < firstChunkItemCount; i++)
            {
                var textStartIndex = reader.ReadInt32();
                var textEndIndex = reader.ReadInt32();
                var codeNameOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;
                reader.BaseStream.Position = fileStartPos + codeNameOffset;
                var codeName = reader.ReadNullTerminatedString(encoding);
                reader.BaseStream.Position = tempOffset;

                Script script = null;
                if (Scripts.Count > i)
                    script = Scripts[i];
                else
                    Scripts.Add(script = new Script(textStartIndex, textEndIndex));
                script.CodeName.SetText(language, codeName);
            }

            reader.BaseStream.Position = fileStartPos + firstChunkSize;

            for (var i = 0; i < secondChunkItemCount; i++)
            {
                var characterNameOffset = reader.ReadInt64();
                var characterPositionOffset = reader.ReadInt64();
                var characterExpressionOffset = reader.ReadInt64();
                var textOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + characterNameOffset;
                var characterName = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = fileStartPos + characterPositionOffset;
                var characterPosition = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = fileStartPos + characterExpressionOffset;
                var characterExpression = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = fileStartPos + textOffset;
                var text = reader.ReadNullTerminatedString(encoding);

                reader.BaseStream.Position = tempOffset;

                ScriptText scriptText = null;
                if (Text.Count > i)
                    scriptText = Text[i];
                else
                    Text.Add(scriptText = new ScriptText());
                scriptText.CharacterName.SetText(language, characterName);
                scriptText.CharacterPosition.SetText(language, characterPosition);
                scriptText.CharacterExpression.SetText(language, characterExpression);
                scriptText.Text.SetText(language, text);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            const int firstChunkOffsetsItemSize = 4 + 4 + 8;
            const int secondChunkOffsetsItemSize = 8 * 4;

            var fileStartPos = writer.BaseStream.Position;
            writer.Write((long) 0);

            writer.Write(Scripts.Count);
            writer.Write(Text.Count);

            var firstChunkOffsetsOffset = writer.BaseStream.Position;
            long firstChunkOffsetsSize = Scripts.Count * firstChunkOffsetsItemSize;
            writer.Write(new byte[firstChunkOffsetsSize]);

            for (var i = 0; i < Scripts.Count; i++)
            {
                var script = Scripts[i];

                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = firstChunkOffsetsOffset + i * firstChunkOffsetsItemSize;
                writer.Write(script.TextStartIndex);
                writer.Write(script.TextEndIndex);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(script.CodeName.GetText(language), encoding);
            }

            var temp = writer.BaseStream.Position;
            writer.BaseStream.Position = fileStartPos;
            writer.Write(temp - fileStartPos);
            writer.BaseStream.Position = temp;

            var secondChunkOffsetsOffset = writer.BaseStream.Position;
            long secondChunkOffsetsSize = Text.Count * secondChunkOffsetsItemSize;
            writer.Write(new byte[secondChunkOffsetsSize]);

            for (var i = 0; i < Text.Count; i++)
            {
                var scriptText = Text[i];

                var characterNameLen = GetStringSize(scriptText.CharacterName.GetText(language), encoding);
                var characterPositionLen = GetStringSize(scriptText.CharacterPosition.GetText(language), encoding);
                var characterExpressionLen = GetStringSize(scriptText.CharacterExpression.GetText(language), encoding);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = secondChunkOffsetsOffset + i * secondChunkOffsetsItemSize;
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + characterNameLen);
                writer.WriteOffset(fileStartPos, tempOffset + characterNameLen + characterPositionLen);
                writer.WriteOffset(fileStartPos,
                    tempOffset + characterNameLen + characterPositionLen + characterExpressionLen);
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
            public Script(int textStartIndex, int textEndIndex)
            {
                TextStartIndex = textStartIndex;
                TextEndIndex = textEndIndex;
                CodeName = new Localized_Text();
            }

            public Localized_Text CodeName { get; set; }
            public int TextStartIndex { get; set; }
            public int TextEndIndex { get; set; }

            public override string ToString()
            {
                return "codeName: '" + CodeName + "' textStartIndex: " + TextEndIndex + " textEndIndex: " +
                       TextEndIndex;
            }
        }

        public class ScriptText
        {
            public ScriptText()
            {
                CharacterName = new Localized_Text();
                CharacterPosition = new Localized_Text();
                CharacterExpression = new Localized_Text();
                Text = new Localized_Text();
            }

            public Localized_Text CharacterName { get; set; }
            public Localized_Text CharacterPosition { get; set; }
            public Localized_Text CharacterExpression { get; set; }
            public Localized_Text Text { get; set; }

            public override string ToString()
            {
                return "characterName: '" + CharacterName + "' characterPosition: '" + CharacterPosition +
                       "' characterExpression: '" + CharacterExpression + "' text: '" + Text + "'";
            }
        }
    }
}