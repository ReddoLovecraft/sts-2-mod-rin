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
    public sealed class MdFormPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/MFP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/MFP64.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IgnitePower>()];
        public MdFormPower() { }
        private int hitCount=0;
        public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	    {
		    if (target == base.Owner && dealer != null &&dealer!=base.Owner&& (props.IsPoweredAttack_() ))
		    {
			 hitCount++;
		    }
	    }
        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side != CombatSide.Player)
            {
                return;
            }
            if(Amount>1)
            await PowerCmd.Decrement(this);
            else
            {
                await CreatureCmd.Kill(this.Owner);
            }
            await PowerCmd.Apply<MultiplierDamagePower>(base.Owner, hitCount*2, null, null);
            hitCount = 0;
        }
    }

}