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
using Patchouib.Scrpits.Main;

namespace TH_Rin.Scrpits.Powers
{
	public sealed class IgnitePower : HealthBarOverlayPowerModel
	{
		public override PowerType Type => PowerType.Debuff;
		public override PowerStackType StackType => PowerStackType.Counter;
		public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
		public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/IP32.png";
		public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/IP64.png";
		public IgnitePower() { }
		public async Task TriggerEffect()
		{
			this.Flash();
			if(base.Owner.HasPower<IntegrityPower>()&&!Owner.HasPower<WildfireInJulyPower>())
			{
              await Owner.GetPower<IntegrityPower>().TriggerLostMoreAmount(5);
			}
			await CreatureCmd.Damage(new BlockingPlayerChoiceContext(), base.Owner, base.Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
			if (base.Owner.IsAlive)
			{
				await PowerCmd.ModifyAmount(this,1,null,null);
			}
			else
			{
				await Cmd.CustomScaledWait(0.1f, 0.25f);
			}
		}
	   public override async Task AfterSideTurnStart(CombatSide side, CombatState combatState)
	{
		if (side != base.Owner.Side)
		{
			return;
		}
		 await TriggerEffect();
	}

		public override Color GetHealthBarOverlayColor()
		{
			return new Color(0.641f, 0.219f, 0.0f, 1.0f);
		}
	}
	

}
