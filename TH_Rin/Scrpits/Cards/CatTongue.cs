using BaseLib.Utils;
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
public class CatTongue : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<IgnitePower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3),new DynamicVar("Power", 4),];
	public CatTongue() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		IEnumerable<CardModel> cards = (await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.IntValue, base.Owner)).Where((CardModel c) => c is RinCardModel rc&&rc.IsFireCard);
		int amount=cards.Count();
		await CardCmd.Discard(choiceContext, cards);
		for(int i=0;i<amount;i++)
		{
			await PowerCmd.Apply<IgnitePower>(Owner.Creature.CombatState.HittableEnemies,this.DynamicVars["Power"].IntValue,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars["Power"].UpgradeValueBy(2);
	}
}

}
