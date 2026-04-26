using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class WraithFire : RinCardModel
{
      protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[4]
        {
		  HoverTipFactory.FromPower<WeakPower>(),
		  HoverTipFactory.FromPower<VulnerablePower>(),
          HoverTipFactory.FromPower<WraithPower>(),
		  HoverTipFactory.FromPower<IgnitePower>()
        });

    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public WraithFire() : base(0, CardType.Skill, CardRarity.Ancient, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		 await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		 await PowerCmd.Apply<WraithPower>(base.Owner.Creature, this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		 foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		 {
			if(mos.IsAlive)
			{
				await PowerCmd.Apply<WeakPower>(mos, this.DynamicVars.Cards.IntValue, Owner.Creature,this);
				await PowerCmd.Apply<VulnerablePower>(mos, this.DynamicVars.Cards.IntValue, Owner.Creature,this);
			}
		 }
	}
	public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card == this)
		{
			 foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		 {
			if(mos.IsAlive)
			{
				await Cmd.Wait(0.25f);
				int cnt=((RinCharacter)Owner.Character).FireGeneratedCount();
				await PowerCmd.Apply<IgnitePower>(mos, cnt, Owner.Creature,this);
			}
		}
		}
	}
	
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1); 
	}
}

}
