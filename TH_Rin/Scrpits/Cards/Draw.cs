using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public sealed class Draw : RinCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(1) };

    public Draw()
        :base(-1, CardType.Status, CardRarity.Status, TargetType.None)
    {
    }
   
}

}
