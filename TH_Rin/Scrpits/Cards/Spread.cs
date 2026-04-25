using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
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
public class Spread : RinCardModel
{
 
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain];
	public Spread() : base(0, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
	{
	}
	private static void DoHackyThingsForSpecificPowers(PowerModel power)
	{
		if (power is ITemporaryPower temporaryPower)
		{
			temporaryPower.IgnoreNextInstance();
		}
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		List<PowerModel> originalDebuffs = (from p in cardPlay.Target.Powers
			where p.TypeForCurrentAmount == PowerType.Debuff
			select (PowerModel)p.ClonePreservingMutability()).ToList();
		foreach (Creature enemy in base.CombatState.HittableEnemies)
		{
			if (enemy == cardPlay.Target&&!IsUpgraded)
			{
				continue;
			}
			foreach (PowerModel item in originalDebuffs)
			{
				PowerModel powerById = enemy.GetPowerById(item.Id);
				if (powerById != null && !powerById.IsInstanced)
				{
					DoHackyThingsForSpecificPowers(powerById);
					await PowerCmd.ModifyAmount(powerById, item.Amount, base.Owner.Creature, this);
				}
				else
				{
					PowerModel power = (PowerModel)item.ClonePreservingMutability();
					DoHackyThingsForSpecificPowers(power);
					await PowerCmd.Apply(power, enemy, item.Amount, base.Owner.Creature, this);
				}
			}
		}
		
	}
	protected override void OnUpgrade()
	{

	}
}

}
