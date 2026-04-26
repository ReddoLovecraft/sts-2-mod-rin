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
public class Focus : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<IgnitePower>(),
          HoverTipFactory.FromPower<WeakPower>(),
          HoverTipFactory.FromPower<VulnerablePower>()
        });
  protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(11),new DynamicVar("Power",1)];
	public Focus() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		VfxCmd.PlayOnCreatureCenter(base.Owner.Creature, "vfx/vfx_flying_slash");
		await CreatureCmd.LoseBlock(cardPlay.Target, cardPlay.Target.Block);
		if (cardPlay.Target.HasPower<ArtifactPower>())
		{
			await PowerCmd.Remove<ArtifactPower>(cardPlay.Target);
		}
		await PowerCmd.Apply<IgnitePower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		await PowerCmd.Apply<WeakPower>(cardPlay.Target,this.DynamicVars["Power"].IntValue,Owner.Creature,this);
		await PowerCmd.Apply<VulnerablePower>(cardPlay.Target,this.DynamicVars["Power"].IntValue,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		this.DynamicVars.Cards.UpgradeValueBy(2);
		this.DynamicVars["Power"].UpgradeValueBy(1);
	}
}

}
