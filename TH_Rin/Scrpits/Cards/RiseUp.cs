using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards;
[Pool(typeof(RinCardPool))]
public class RiseUp : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2),new EnergyVar(2)];
	  protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
{
         base.EnergyHoverTip
});
	public RiseUp() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllAllies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
			where c != null && c.IsAlive && c.IsPlayer&&c.Player!=base.Owner
			select c;
		foreach (Creature item in enumerable)
		{
			await CardPileCmd.Draw(choiceContext, base.DynamicVars.Cards.BaseValue, item.Player);
			await PlayerCmd.GainEnergy(base.DynamicVars.Energy.BaseValue,item.Player);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars.Energy.UpgradeValueBy(1);
	}
}
