namespace Yu_Gi_Oh.File_Handling.Utility
{
    public class Localized_Offset
    {
        public Localized_Offset()
        {
            Universal = -1;
        }

        public long Universal { get; set; }

        public long English { get; set; }
        public long French { get; set; }
        public long German { get; set; }
        public long Italian { get; set; }
        public long Spanish { get; set; }

        public void SetValue(Localized_Text.Language language, long value)
        {
            switch (language)
            {
                case Localized_Text.Language.English:
                    English = value;
                    break;
                case Localized_Text.Language.French:
                    French = value;
                    break;
                case Localized_Text.Language.German:
                    German = value;
                    break;
                case Localized_Text.Language.Italian:
                    Italian = value;
                    break;
                case Localized_Text.Language.Spanish:
                    Spanish = value;
                    break;
                case Localized_Text.Language.Unknown:
                    Universal = value;
                    break;
                default:
                    Universal = value;
                    break;
            }
        }

        public long GetValue(Localized_Text.Language language)
        {
            switch (language)
            {
                case Localized_Text.Language.English: return English;
                case Localized_Text.Language.French: return French;
                case Localized_Text.Language.German: return German;
                case Localized_Text.Language.Italian: return Italian;
                case Localized_Text.Language.Spanish: return Spanish;
                case Localized_Text.Language.Unknown: return Universal;
                default: return Universal;
            }
        }
    }
}