using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Relics;
using Patchouib.Scrpits.Main;
using MegaCrit.Sts2.Core.Models.Monsters;

namespace TH_Rin.Scrpits.Powers
{
	public sealed class IntegrityPower : CustomPowerModel
	{
		public override PowerType Type => PowerType.Buff;
		public override PowerStackType StackType => PowerStackType.Counter;
		public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
		public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/IP232.png";
		public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/IP264.png";
		public IntegrityPower() { }
		public async Task TriggerLostMoreAmount(int additionalAmount)
		{
			await PowerCmd.ModifyAmount(this,-additionalAmount,null,null);
		}
		public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (!wasRemovalPrevented && base.Owner == target)
		{
			if(Owner.Monster!=null)
			{
			IEnumerable<Creature> enumerable = from c in base.CombatState.PlayerCreatures
			where c != null && c.IsAlive && c.IsPlayer
			select c;
			foreach (Creature item in enumerable)
			{
			  bool flag=item.HasPower<PreventRotPower>();
			  if(!flag)
			  	Tools.AddCorpseToBarrow(item.Player,Tools.GetCorpseCard(Owner.Monster,item.Player,this.Amount,8));
			  else
			 	Tools.AddCorpseToBarrow(item.Player,Tools.GetCorpseCard(Owner.Monster,item.Player,this.Amount,999,false));
			}
			// if(Owner.Monster is DecimillipedeSegment)
			// {
			// 	await CreatureCmd.Kill(Owner);
			// }
			}
		}
	}
		public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target == base.Owner && dealer != null && props.IsPoweredAttack()&&amount>0)
		{
			Flash();
			if(amount>=target.MaxHp/4)
			{
				await TriggerLostMoreAmount(5);
			}
			else
			{
				await PowerCmd.Decrement(this);
			}
		}
	}
		// public Color GetHealthBarOverlayColor()
		// {
		// 	return new Color(0.774f, 0.681f, 0.078f, 1.0f);
		// }

		// int IHealthBarOverlayPower.GetHealthBarOverlayValue(Creature owner)
		// {
		// 	return Amount;
		// }

		// bool IHealthBarOverlayPower.IsOverlayFromEnd()
		// {
		// 	return false;
		// }

		// bool IHealthBarOverlayPower.IsOverlayLethal(Creature owner)
		// {
		//    return true;
		// }
	}
	

}
