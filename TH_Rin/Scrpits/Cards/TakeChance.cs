using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
public class TakeChance : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          base.EnergyHoverTip
        });
		protected override bool ShouldGlowGoldInternal => Success;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2),new EnergyVar(2)];
	public TakeChance() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if(Success)
		{
		  SfxCmd.Play(RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/cg.wav"));
          await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue,Owner);
		  await CardPileCmd.Draw(choiceContext,this.DynamicVars.Cards.IntValue,Owner);
		}
	}
	private bool Success
	{
		get
		{
			int num = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(base.CombatState) && e.CardPlay.Card.Owner == base.Owner);
			return num < 3;
		}
	}
		protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars.Energy.UpgradeValueBy(1);
	}
}

}
