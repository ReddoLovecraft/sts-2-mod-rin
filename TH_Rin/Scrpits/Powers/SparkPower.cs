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
    public sealed class SparkPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/SP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/SP64.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IgnitePower>()];
		protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4)];
        public SparkPower() { }
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != base.Owner.Player)
            {
                return;
            }
            int totalIncreament=0;
            foreach(Creature mos in Owner.CombatState.HittableEnemies)
            {
                if(mos.IsAlive)
                {
                    await PowerCmd.Apply<IgnitePower>(mos,this.Amount,Owner,null);
                    if(mos.HasPower<IgnitePower>())
                    {
                        totalIncreament+=mos.GetPowerAmount<IgnitePower>();
                    }
                    break;
                }
            }
           await PowerCmd.ModifyAmount(this,totalIncreament,null,null);
        }

	}

}