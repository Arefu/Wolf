using System.Collections.Generic;

namespace Yu_Gi_Oh.File_Handling.Miscellaneous_Files
{
    public class CardCollection
    {
        public CardCollection()
        {
            CardIds = new List<short>();
        }

        public List<short> CardIds { get; set; }

        public void Add(short cardId)
        {
            CardIds.Add(cardId);
        }

        public void Remove(short cardId)
        {
            CardIds.Remove(cardId);
        }

        public void RemoveAll(short cardId)
        {
            while (CardIds.Remove(cardId))
            {
            }
        }

        public void Clear()
        {
            CardIds.Clear();
        }

        public void Sort()
        {
            CardIds.Sort();
        }
    }
}