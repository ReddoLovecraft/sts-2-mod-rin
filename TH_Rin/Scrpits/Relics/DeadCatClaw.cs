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
using MegaCrit.Sts2.Core.ValueProps;
using Patchouib.Scrpits.Main;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Relics
{
[Pool(typeof(RinRelicPool))]
public class DeadCatClaw : CustomRelicModel,IRightCilckable
{
      protected override IEnumerable<DynamicVar> CanonicalVars => (new DynamicVar[1]
	{
		new CardsVar(0)
	});
     private int cnt=0;
     private int maxCnt=0;
    [SavedProperty]
    public virtual int Cnt
    {
        get{return cnt;}
        set
        {
            AssertMutable();
			cnt=value;
			InvokeDisplayAmountChanged();
        }
    }
     [SavedProperty]
    public virtual int MaxCnt
    {
        get{return maxCnt;}
        set
        {
            AssertMutable();
			maxCnt=value;
            this.DynamicVars.Cards.BaseValue=maxCnt;
			InvokeDisplayAmountChanged();
        }
    }
    public override int DisplayAmount
	{
		get
		{
			return Cnt;
		}
	}
     public void Refresh() { InvokeDisplayAmountChanged(); }
     public override bool ShowCounter => true;
	public override string PackedIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    protected override string PackedIconOutlinePath => $"res://TH_Rin/ArtWorks/Relics/Outlines/{Id.Entry}.png";
    protected override string BigIconPath => $"res://TH_Rin/ArtWorks/Relics/{Id.Entry}.png";
    public override RelicRarity Rarity => RelicRarity.Rare;

    public async Task OnRightClick(PlayerChoiceContext context)
    {
            if(Owner.Creature.CombatState!=null)
            {
                 await CreatureCmd.LoseMaxHp(context,Owner.Creature,1,false);
                 Cnt+=3;
                 MaxCnt+=3;
                 Refresh();
                 this.Flash();
            }
    }
        public override decimal ModifyHpLostBeforeOsty(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (dealer==null||dealer == base.Owner.Creature)
		{
			return amount;
		}
        if (target != base.Owner.Creature)
		{
			return amount;
		}
		if (amount < 1m||Cnt==0)
		{
			return amount;
		}
        this.Flash();
        if(this.Cnt>amount)
        {
            Cnt-=(int)amount;
            this.Refresh();
            return 0;
        }
        else
        {
            Cnt=0;
            this.Refresh();
		    return amount-Cnt;
        }
	}

	public override Task AfterModifyingHpLostBeforeOsty()
	{
		Flash();
		return Task.CompletedTask;
	}
     public override async Task BeforeCombatStart()
    {
        this.Cnt=MaxCnt;
        this.Flash();
        Refresh();
    }
}
}
