using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class Satori : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<DexterityPower>(),
		  HoverTipFactory.FromPower<StrengthPower>(),
		  this.EnergyHoverTip
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2),new EnergyVar(2)];
	public Satori() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		bool attackFlag=cardPlay.Target.Monster.NextMove.Intents.Any(delegate(AbstractIntent intent)
		{
			IntentType intentType = intent.IntentType;
			return intentType == IntentType.Attack || intentType == IntentType.DeathBlow ? true : false;
		});
		if(attackFlag)
		await PowerCmd.Apply<DexterityPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		bool DefendFlag=cardPlay.Target.Monster.NextMove.Intents.Any(delegate(AbstractIntent intent)
		{
			IntentType intentType = intent.IntentType;
			return intentType == IntentType.Defend ? true : false;
		});
		if(DefendFlag)
		await PowerCmd.Apply<StrengthPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		bool OtherFlag=cardPlay.Target.Monster.NextMove.Intents.Any(delegate(AbstractIntent intent)
		{
			IntentType intentType = intent.IntentType;
			return intentType != IntentType.Attack && intentType != IntentType.DeathBlow && intentType != IntentType.Defend ? true : false;
		});
		if(OtherFlag)
		{
			await PlayerCmd.GainEnergy(this.DynamicVars.Energy.IntValue,Owner);
			await CardPileCmd.Draw(choiceContext,this.DynamicVars.Cards.IntValue,Owner);
		}
		
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars.Energy.UpgradeValueBy(1);
	}
}

}
