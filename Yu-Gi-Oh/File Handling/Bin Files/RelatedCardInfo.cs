namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     A class that holds related card information
    /// </summary>
    public class RelatedCardInfo
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="card">The card info you're wanting related info for.</param>
        /// <param name="tagInfo">The taginfo of the card you're wanting related info for.</param>
        public RelatedCardInfo(Card_Info card, Card_Tag_Info tagInfo)
        {
            Card = card;
            TagInfo = tagInfo;
        }

        /// <summary>
        ///     The related card info.
        /// </summary>
        public Card_Info Card { get; set; }

        /// <summary>
        ///     The tag info for the related card.
        /// </summary>
        public Card_Tag_Info TagInfo { get; set; }
    }
}