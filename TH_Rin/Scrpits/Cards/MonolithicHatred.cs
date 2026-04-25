using BaseLib.Extensions;
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
public class MonolithicHatred : RinCardModel
{
	public override bool GainsBlock => true;
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<WraithPower>()
        });
	public MonolithicHatred() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int add=Owner.HasPower<WraithPower>()?Owner.Creature.GetPowerAmount<WraithPower>():0;
		await DamageCmd.Attack(add) .FromCard(this) .Targeting(cardPlay.Target).Execute(choiceContext);
		await CreatureCmd.GainBlock(Owner.Creature,new BlockVar(add,ValueProp.Move),cardPlay);
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
