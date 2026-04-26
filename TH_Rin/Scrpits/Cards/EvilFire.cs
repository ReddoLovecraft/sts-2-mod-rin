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
public class EvilFire : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<IgnitePower>(),
          HoverTipFactory.FromPower<WeakPower>(),
          HoverTipFactory.FromPower<VulnerablePower>()
        });
  protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2),new DynamicVar("Power",5)];
	public EvilFire() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<WeakPower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		await PowerCmd.Apply<VulnerablePower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		await PowerCmd.Apply<IgnitePower>(cardPlay.Target,this.DynamicVars["Power"].IntValue,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars["Power"].UpgradeValueBy(2);
	}
}

}
