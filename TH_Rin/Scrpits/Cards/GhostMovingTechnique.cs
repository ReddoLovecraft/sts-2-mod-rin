using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class GhostMovingTechnique : RinCardModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public GhostMovingTechnique() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int num = Math.Min(base.DynamicVars.Cards.IntValue, 10 - PileType.Hand.GetPile(base.Owner).Cards.Count);
		if (num > 0)
		{
			await CardPileCmd.Add(await CardSelectCmd.FromSimpleGrid(choiceContext, PileType.Discard.GetPile(base.Owner).Cards, base.Owner, new CardSelectorPrefs(base.SelectionScreenPrompt, 0,num)), PileType.Hand);
		}
	}
	protected override void OnUpgrade()
	{
		this.AddKeyword(CardKeyword.Retain);
		DynamicVars.Cards.UpgradeValueBy(1);
	}
}

}
