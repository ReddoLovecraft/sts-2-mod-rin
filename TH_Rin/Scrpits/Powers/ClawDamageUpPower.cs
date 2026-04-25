using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class ClawDamageUpPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/CDUP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/CDUP64.png";
        public ClawDamageUpPower() { }
        public override decimal ModifyHpLostBeforeOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (dealer==null||dealer != base.Owner)
		{
			return amount;
		}
        if(cardSource==null)
        {
            return amount;
        }
        if(!cardSource.Id.Entry.Contains("_CLAW"))
        {
            return amount;
        }
		return amount+Amount;
	}
	}

}