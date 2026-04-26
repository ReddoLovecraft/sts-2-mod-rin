using System.Linq;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
    public sealed class SilentTailingPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/STP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/STP64.png";
        public SilentTailingPower() { }
        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side == base.Owner.Side)
            {
                int attackCardCount = CombatManager.Instance.History.CardPlaysFinished.Count(
                    (CardPlayFinishedEntry e) => 
                        e.CardPlay.Card.Type == CardType.Attack && 
                        e.CardPlay.Card.Owner == base.Owner.Player && 
                        e.HappenedThisTurn(base.CombatState));
                if (attackCardCount == 0)
                {
                    Flash();
                    await PowerCmd.Apply<MultiplierDamagePower>(Owner, this.Amount, null, null);
                }
                await PowerCmd.Remove(this);
            }
        }
    }
}