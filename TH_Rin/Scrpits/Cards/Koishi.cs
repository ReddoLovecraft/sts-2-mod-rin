using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class Koishi : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[4]
        {
          HoverTipFactory.Static(StaticHoverTip.Transform),
		  HoverTipFactory.FromCard<RollingBoulder>(),
		  HoverTipFactory.FromCard<PrimalForce>(),
		  HoverTipFactory.FromCard<GiantRock>(),
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];
	public Koishi() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		List<CardModel> list = PileType.Hand.GetPile(base.Owner).Cards.ToList();
		int cardCount = list.Count;
		foreach (CardModel item in list)
		{
			switch(item.Type)
			{
				case CardType.Attack:
					CardModel c1 = base.CombatState.CreateCard<GiantRock>(base.Owner);
					if(IsUpgraded)
					c1.EnergyCost.SetThisTurnOrUntilPlayed(0);
					await CardCmd.Transform(item, c1);
					break;
				case CardType.Skill:
					CardModel c2 = base.CombatState.CreateCard<PrimalForce>(base.Owner);
					if(IsUpgraded)
					c2.EnergyCost.SetThisTurnOrUntilPlayed(0);
					await CardCmd.Transform(item, c2);
					break;
				case CardType.Power:
					CardModel c3 = base.CombatState.CreateCard<RollingBoulder>(base.Owner);
					if(IsUpgraded)
					c3.EnergyCost.SetThisTurnOrUntilPlayed(0);
					await CardCmd.Transform(item, c3);
					break;
				default:
					break;
			}
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(2);
	}
}

}
