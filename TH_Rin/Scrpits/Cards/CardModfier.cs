using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TH_Rin.Scrpits.Cards
{
    public class CardModfier 
    {
        [CustomEnum("NOROT")]
        [KeywordProperties(AutoKeywordPosition.After)]
        public static CardKeyword NorotKeyword;
        [CustomEnum]
        public static CardTag NorotTag;
    }
}