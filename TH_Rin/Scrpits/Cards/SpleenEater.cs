using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
public class SpleenEater : RinCardModel
{
	   public override bool GainsBlock => true;
	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<WraithPower>(),
          HoverTipFactory.FromPower<WeakPower>(),
          HoverTipFactory.FromPower<VulnerablePower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Move),new CardsVar(2)];
	protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<WraithPower>();

	public SpleenEater() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if (cardPlay.Target == null)
		{
			return;
		}
		IEnumerable<DamageResult> results = await CreatureCmd.Damage(choiceContext, cardPlay.Target, base.DynamicVars.Damage.BaseValue, ValueProp.Move, base.Owner.Creature, this);
		int unblockedDamage = (int)results.Sum((DamageResult r) => r.UnblockedDamage);
		if (unblockedDamage > 0&&cardPlay.Target.IsAlive)
		{
			await PowerCmd.Apply<VulnerablePower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
			await PowerCmd.Apply<WeakPower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		}
		else if(ShouldGlowGoldInternal&&cardPlay.Target.IsAlive)
		{
			await PowerCmd.Apply<VulnerablePower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
			await PowerCmd.Apply<WeakPower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2); 
		DynamicVars.Cards.UpgradeValueBy(1); 
	}
}

}
