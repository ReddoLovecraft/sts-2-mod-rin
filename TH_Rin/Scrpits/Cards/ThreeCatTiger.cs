using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class ThreeCatTiger : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<StrengthPower>()
        });
  protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
	public ThreeCatTiger() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if(Owner.HasPower<StrengthPower>())
		{
           await PowerCmd.ModifyAmount(Owner.Creature.GetPower<StrengthPower>(),this.DynamicVars.Cards.IntValue*Owner.Creature.GetPowerAmount<StrengthPower>(),Owner.Creature,this);
		}
		else
		{
			await PowerCmd.Apply<StrengthPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
