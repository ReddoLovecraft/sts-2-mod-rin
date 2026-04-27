using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using Patchouib.Scrpits.Main;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Relics
{
[Pool(typeof(RinRelicPool))]
public class MeatCooker : CustomRelicModel
{
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<IgnitePower>(),HoverTipFactory.FromPower<IntegrityPower>()];
    	public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
	{
		if (target.Side != base.Owner.Creature.Side&&target.HasPower<IgnitePower>())
		{
			Flash();
			await CreatureCmd.Heal(Owner.Creature,3);
		}
	}
}
}
