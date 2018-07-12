using System.Collections.Generic;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class GameFileRepository : IGameFileRepository
    {
        private readonly Manager _manager;

        public GameFileRepository(Manager manager)
        {
            _manager = manager;
        }

        public List<Card> GetAllCards()
        {
            return _manager.CardManager.Cards
                .Where(x => x.Value.CardId != 0)
                .Select(x => new Card()
                {
                    Id = x.Value.CardId,
                    Name = x.Value.Name.GetText(Localized_Text.Language.English)
                }).ToList();
        }
    }
}
