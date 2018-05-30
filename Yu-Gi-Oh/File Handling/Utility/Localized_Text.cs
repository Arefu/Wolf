namespace Yu_Gi_Oh.File_Handling.Utility
{
    public class Localized_Text
    {
        public enum Language
        {
            Unknown,

            English,
            French,
            German,
            Italian,
            Spanish
        }

        private string english;

        private string french;

        private string german;

        private string italian;
        private Language lastLanguageSet = Language.Unknown;

        private string spanish;
        public string Universal { get; set; }

        public string English
        {
            get => english;
            set
            {
                english = value;
                lastLanguageSet = Language.English;
            }
        }

        public string French
        {
            get => french;
            set
            {
                french = value;
                lastLanguageSet = Language.French;
            }
        }

        public string German
        {
            get => german;
            set
            {
                german = value;
                lastLanguageSet = Language.German;
            }
        }

        public string Italian
        {
            get => italian;
            set
            {
                italian = value;
                lastLanguageSet = Language.Italian;
            }
        }

        public string Spanish
        {
            get => spanish;
            set
            {
                spanish = value;
                lastLanguageSet = Language.Spanish;
            }
        }

        public void SetText(Language language, string value)
        {
            switch (language)
            {
                case Language.English:
                    English = value;
                    break;
                case Language.French:
                    French = value;
                    break;
                case Language.German:
                    German = value;
                    break;
                case Language.Italian:
                    Italian = value;
                    break;
                case Language.Spanish:
                    Spanish = value;
                    break;
                case Language.Unknown:
                    Universal = value;
                    break;
                default:
                    Universal = value;
                    break;
            }
        }

        public string GetText(Language language)
        {
            switch (language)
            {
                case Language.English: return English;
                case Language.French: return French;
                case Language.German: return German;
                case Language.Italian: return Italian;
                case Language.Spanish: return Spanish;
                case Language.Unknown: return Universal;
                default: return Universal;
            }
        }

        public override string ToString()
        {
            return English ?? GetText(lastLanguageSet);
        }
    }
}