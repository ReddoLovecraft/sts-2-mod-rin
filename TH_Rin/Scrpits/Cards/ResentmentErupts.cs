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
public class ResentmentErupts : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<WraithPower>()
        });
	protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<WraithPower>();
	public ResentmentErupts() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int wraithCount = Owner.Creature.GetPowerAmount<WraithPower>();
		if(wraithCount > 0)
		foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies.ToList())
		{
           if(mos.IsAlive)
		   {
			 int lossValue=wraithCount*mos.MaxHp/100;
			 await CreatureCmd.Damage(choiceContext,mos,lossValue,ValueProp.Unblockable|ValueProp.Unpowered,Owner.Creature,null);
		   }
		}
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
