using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using TH_Rin.Scrpits.Powers;


namespace TH_Rin.Scripts.Main
{
    public abstract class RinCardModel : CustomCardModel
    {
        public override string PortraitPath => $"res://TH_Rin/ArtWorks/Cards/{Id.Entry}.png";
        public RinCardModel(int baseCost, CardType type, CardRarity rarity, TargetType target, bool showInCardLibrary = true, bool autoAdd = true)
     : base(baseCost, type, rarity, target, showInCardLibrary)
        {
            if (autoAdd)
            {
                CustomContentDictionary.AddModel(GetType());
            }
        }
        public bool IsFireCard => Tools.HasPowerHoverTip<IgnitePower>(ExtraHoverTips);

    }
  
}
