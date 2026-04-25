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
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class ThiefCatPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool IsInstanced => true;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/TCP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/TCP64.png";
        public ThiefCatPower() { }
       public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
	    {
		if (player != base.Owner.Player)
		{
			return false;
		}
		if (room == null)
		{
			return false;
		}
        if (room.RoomType == RoomType.Elite||room.RoomType == RoomType.Boss)
        {
            rewards.Add(new RelicReward(player));
            rewards.Add(new CardReward(CardCreationOptions.ForRoom(base.Owner.Player, RoomType.Boss), 3, player));
            return true;
        }
        else
        {
            rewards.Add(new CardReward(CardCreationOptions.ForRoom(base.Owner.Player, RoomType.Monster), 3, player));
            return true;
        }
	    }
    }
}