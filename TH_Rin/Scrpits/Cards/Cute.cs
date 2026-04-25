using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
public class Cute : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WeakPower>(),
          HoverTipFactory.FromPower<StrengthPower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1), new DynamicVar("Power", 8)];
	public Cute() : base(1, CardType.Skill, CardRarity.Common, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		SfxCmd.Play(RinInit.ToModSfxPath("TH_Rin/ArtWorks/SFX/characterselect.wav"));
		 int attackCardCount = CombatManager.Instance.History.CardPlaysFinished.Count(
                    (CardPlayFinishedEntry e) => 
                        e.CardPlay.Card.Type == CardType.Attack && 
                        e.CardPlay.Card.Owner == base.Owner && 
                        e.HappenedThisTurn(base.CombatState));
         if (attackCardCount == 0)
        {
			await PowerCmd.Apply<WeakPower>(Owner.Creature.CombatState.HittableEnemies,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
			await PowerCmd.Apply<PiercingWailPower>(Owner.Creature.CombatState.HittableEnemies,this.DynamicVars["Power"].IntValue,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars["Power"].UpgradeValueBy(2);
	}
}

}
