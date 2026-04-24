using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class TestCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
        	Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0) };
	public TestCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}

	protected override void RefreshCorpseVars()
	{
		DynamicVars.Cards.BaseValue = RotCount;
	}
	
}

}
