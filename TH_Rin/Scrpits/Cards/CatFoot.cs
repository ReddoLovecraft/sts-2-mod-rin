using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class CatFoot : RinCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
	public CatFoot() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IEnumerable<CardModel> cards = await CardPileCmd.Draw(choiceContext,this.DynamicVars.Cards.IntValue,Owner);
		foreach(var item in cards)
		{
			item.SetToFreeThisTurn();
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
	}
}

}
