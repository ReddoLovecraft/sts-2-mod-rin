using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class Bury : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
		  HoverTipFactory.FromKeyword(CardKeyword.Exhaust),
          HoverTipFactory.FromPower<WraithPower>(),
          HoverTipFactory.FromPower<StrengthPower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
	public Bury() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
		CardModel card = (await CardSelectCmd.FromHand(choiceContext, base.Owner, prefs, (CardModel c) => true, this)).FirstOrDefault();
		if (card != null)
		{
			if(card.Keywords.Contains(CardKeyword.Unplayable))
			{
                await CardCmd.Exhaust(choiceContext,card);
			}
			else
			{
				CardCmd.ApplyKeyword(card, CardKeyword.Exhaust);
				await CardCmd.AutoPlay(choiceContext, card, null);
			}
			if(card.Type==CardType.Status||card.Type==CardType.Curse)
			{
				await PowerCmd.Apply<StrengthPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
			}
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(2);
		this.AddKeyword(CardKeyword.Retain);
	}
}

}
