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
    public sealed class FireDancePower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/FDP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/FDP64.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IgnitePower>()];
        public FireDancePower() { }
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
	{
		if (cardPlay.Card.Owner==base.Owner.Player&&cardPlay.Card is RinCardModel rc&&rc.IsFireCard)
		{
			this.Flash();
            foreach(Creature mos in Owner.CombatState.HittableEnemies)
            if(mos.IsAlive)
			    await PowerCmd.Apply<IgnitePower>(mos, base.Amount, null, null);
		}
	}
    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card.Owner==base.Owner.Player&&card is RinCardModel rc&&rc.IsFireCard)
		{
			this.Flash();
			foreach(Creature mos in Owner.CombatState.HittableEnemies)
                if(mos.IsAlive)
			    await PowerCmd.Apply<IgnitePower>(mos, base.Amount, null, null);
		}
	}
        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side == base.Owner.Side)
            {
                Flash();
                await PowerCmd.Remove(this);
            }
        }
	}

}