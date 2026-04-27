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
using MegaCrit.Sts2.Core.ValueProps;
using Patchouib.Scrpits.Main;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Relics
{
[Pool(typeof(RinRelicPool))]
public class Cattail : CustomRelicModel
{
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
     public override RelicRarity Rarity => RelicRarity.Common;
     public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (!CombatManager.Instance.IsInProgress)
		{
			await Task.CompletedTask;
			return;
		}
		if (target == base.Owner.Creature)
		{
			await Task.CompletedTask;
			return;
		}
        if(dealer==null||dealer!=base.Owner.Creature)
        {
			await Task.CompletedTask;
			return;
		}
		if(result.UnblockedDamage<=0)
		{
			await Task.CompletedTask;
			return;
		}
        Flash();
		VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_bloody_impact");
		await CreatureCmd.Damage(choiceContext, target, 1, ValueProp.Unblockable | ValueProp.Unpowered,null,null);
        await Task.CompletedTask;
	}

}
}
