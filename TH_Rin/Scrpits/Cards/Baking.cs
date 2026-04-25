using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class Baking : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromCard<Cake>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
	public Baking() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		List<CardModel> list = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner, new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt,0, base.DynamicVars.Cards.IntValue))).ToList();
		foreach (CardModel item in list)
		{
			CardPileAddResult? cardPileAddResult = await CardCmd.TransformTo<Cake>(item);
		}
	}
	protected override void OnUpgrade()
	{
		this.RemoveKeyword(CardKeyword.Exhaust);
	}
}

}
