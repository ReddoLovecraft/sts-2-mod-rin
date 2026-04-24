using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scripts.Powers
{
public sealed class DazePower : RinPowerModel
{
	public override PowerType Type => PowerType.Debuff;
	public override PowerStackType StackType => PowerStackType.Counter;
	public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
    public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/DP32.png";
    public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/DP64.png";
	protected override IEnumerable<IHoverTip> ExtraHoverTips => [StunIntent.GetStaticHoverTip()];
		public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (target == base.Owner && dealer != null &&dealer!=base.Owner&& (props.IsPoweredAttack_() ))
		{
			if(this.Amount<=1)
			{
				await CreatureCmd.Stun(Owner);
				await PowerCmd.Remove(this);
			}
			else await PowerCmd.Decrement(this);
		}
		
	}
}
}



