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
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class CorpseLove : RinCardModel
{
	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WeakPower>(),
          HoverTipFactory.FromPower<VulnerablePower>(),
        });
	protected override bool ShouldGlowGoldInternal
	{
		get
		{
			IBarrowLike? barrow = Tools.GetBarrowRelic(Owner);
			return barrow != null && barrow.CorpseCards.Count > 0;
		}
	}
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public CorpseLove() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<VulnerablePower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		await PowerCmd.Apply<WeakPower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		if(ShouldGlowGoldInternal)
		{
			await PowerCmd.Apply<DebilitatePower>(cardPlay.Target,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		this.DynamicVars.Cards.UpgradeValueBy(1);
	}
}

}
