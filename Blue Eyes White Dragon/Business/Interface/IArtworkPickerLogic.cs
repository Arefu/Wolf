using System.Collections.Generic;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IArtworkPickerLogic
    {
        IEnumerable<ArtworkSearch> SearchCards(string cardName);
    }
}