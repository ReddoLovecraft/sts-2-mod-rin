using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class HumanSoulLantern : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WraithPower>(),
          base.EnergyHoverTip
        });
	protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<WraithPower>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1),new EnergyVar(1),new HpLossVar(10)];
	public HumanSoulLantern() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if(ShouldGlowGoldInternal)
		{
          await Owner.Creature.GetPower<WraithPower>().TriggerExhasut(1);
		  await NWraithOrbVfx.Play(Owner.Creature, cardPlay.Target);
          await CreatureCmd.Damage(choiceContext, cardPlay.Target, DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered,null,null);
		  await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue,Owner);
		}
		else
		{
			await PowerCmd.Apply<WraithPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
			await CardPileCmd.Draw(choiceContext,DynamicVars.Cards.IntValue,Owner);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars.Energy.UpgradeValueBy(1);
	}
}

}
