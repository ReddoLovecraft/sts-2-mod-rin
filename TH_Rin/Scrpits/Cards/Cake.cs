using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class Cake : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          base.EnergyHoverTip
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4),new EnergyVar(1)];
	public Cake() : base(0, CardType.Status, CardRarity.Status, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue,Owner);
		await CreatureCmd.Heal(Owner.Creature,this.DynamicVars.Cards.IntValue);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Energy.UpgradeValueBy(1);
		DynamicVars.Cards.UpgradeValueBy(2);
	}
}

}
