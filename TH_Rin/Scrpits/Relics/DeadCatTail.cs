using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Patchouib.Scrpits.Main;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Relics
{
[Pool(typeof(RinRelicPool))]
public class DeadCatTail : CustomRelicModel
{
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
	{
		if (player != base.Owner)
		{
			return false;
		}
		if (room == null ||  (room.RoomType != RoomType.Monster && room.RoomType != RoomType.Elite && room.RoomType != RoomType.Boss))
		{
			return false;
		}
        	Rng rng = Owner.RunState.Rng.CombatCardGeneration;
            int num = rng.NextInt(101);
            if (num <= 33)
            {
                rewards.Add(new RelicReward(RelicRarity.Uncommon,player));
            }
            else if (num <= 66)
            {
                rewards.Add(new RelicReward(RelicRarity.Rare,player));
            }
		return true;
	}

}
}
