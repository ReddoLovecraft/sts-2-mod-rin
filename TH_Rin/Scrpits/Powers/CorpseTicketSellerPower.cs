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
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scrpits.Relics;


namespace TH_Rin.Scrpits.Powers
{
    public sealed class CorpseTicketSellerPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool IsInstanced => true;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/CTSP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/CTSP64.png";
        protected override IEnumerable<IHoverTip> ExtraHoverTips => [Tools.GetStaticKeyword("Corpse")];
        public CorpseTicketSellerPower() { }
        private int deathNum=0;
        public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
	    {
		    if (target.Side != base.Owner.Side)
            deathNum++;
        }
        public override Task AfterCombatEnd(CombatRoom room)
	    {
        int finalValue=0;
        if(Owner.Player.GetRelic<Barrow>()!=null)
        {
            finalValue=Owner.Player.GetRelic<Barrow>().CorpseCards.Count*deathNum;
        }
        if(finalValue>0)
		    room.AddExtraReward(base.Owner.Player, new GoldReward(finalValue, base.Owner.Player));
		    return Task.CompletedTask;
	    }
	}

}