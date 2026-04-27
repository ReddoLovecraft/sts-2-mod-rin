using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Relics
{
[Pool(typeof(RinRelicPool))]
public class CatCollar : CustomRelicModel
{
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    public override RelicRarity Rarity => RelicRarity.Rare;
	bool flag=true;
    public override bool ShouldDieLate(Creature creature)
	{
		if (creature != base.Owner.Creature)
		{
			return true;
		}
		Rng rng = Owner.RunState.Rng.CombatCardGeneration;
        flag = rng.NextBool();
		return flag;
	}
	public override async Task AfterPreventingDeath(Creature creature)
	{
		if(!flag)
		{
		Flash();
		decimal amount = Math.Max(1m, (decimal)creature.MaxHp *0.1m);
		await CreatureCmd.Heal(creature, amount);	
		}
	}
}
}
