using BaseLib.Utils;
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
public class MarsCat : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<IgnitePower>(),
		  base.EnergyHoverTip
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(9),new EnergyVar(1)];
	public MarsCat() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		(await PowerCmd.Apply<MarsCatPower>(Owner.Creature,this.DynamicVars.Energy.IntValue,Owner.Creature,this)).SetDamage(this.DynamicVars.Cards.IntValue);
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
