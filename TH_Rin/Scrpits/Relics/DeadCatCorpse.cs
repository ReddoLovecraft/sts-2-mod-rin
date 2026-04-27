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
using MegaCrit.Sts2.Core.Saves.Runs;
using Patchouib.Scrpits.Main;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Relics
{
[Pool(typeof(RinRelicPool))]
public class DeadCatCorpse : CustomRelicModel
{
    private int cnt=9;
    [SavedProperty]
    public virtual int  ReviveCount
    {
        get{return cnt;}
        set
        {
            AssertMutable();
			cnt=value;
            this.DynamicVars.Cards.BaseValue=cnt;
			InvokeDisplayAmountChanged();
        }
    }
    public void Refresh() { InvokeDisplayAmountChanged(); }
    public override bool IsUsedUp => ReviveCount<=0;
    public override int DisplayAmount
	{
		get
		{
			return ReviveCount;
		}
	}
    public override bool ShowCounter => true;
     protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[1]
	{
		new CardsVar(9)
	});
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    public override RelicRarity Rarity => RelicRarity.Event;
    public override async Task AfterObtained()
	{
        int dif=Owner.Creature.MaxHp-10;
        if(dif>0)
        {
            await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(),Owner.Creature,dif,false);
        }
        else
        {
            await CreatureCmd.GainMaxHp(Owner.Creature,-dif);
        }
    }
    public override bool ShouldDieLate(Creature creature)
	{
		if (creature != base.Owner.Creature)
		{
			return true;
		}
		return !(ReviveCount>0);
	}
	public override async Task AfterPreventingDeath(Creature creature)
	{
		if(ReviveCount>0)
		{
		  Flash();
		  await CreatureCmd.Heal(creature,creature.MaxHp);	
		  ReviveCount--;
          DynamicVars.Cards.BaseValue=ReviveCount;
		  Refresh();
		}
	}
}
}
