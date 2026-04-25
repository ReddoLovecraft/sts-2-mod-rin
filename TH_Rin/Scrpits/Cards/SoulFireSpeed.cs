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
public class SoulFireSpeed : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WraithPower>(),
		  base.EnergyHoverTip
        });
	protected override bool IsPlayable => Owner.Creature.GetPowerAmount<WraithPower>()>this.DynamicVars.Cards.IntValue;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1),new EnergyVar(2)];
	public SoulFireSpeed() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if(Owner.HasPower<WraithPower>())
		{
			await Owner.Creature.GetPower<WraithPower>().TriggerExhasut(this.DynamicVars.Cards.IntValue);
		}
		await CardPileCmd.Draw(choiceContext,this.DynamicVars.Cards.IntValue,Owner);
		await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue,Owner);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
	}
}

}
