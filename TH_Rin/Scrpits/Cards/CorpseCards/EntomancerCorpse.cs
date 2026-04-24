using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scripts.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class EntomancerCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
        StunIntent.GetStaticHoverTip(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public EntomancerCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 7m / GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
	 public override async Task TriggerWhenCombatStart()
    {
       foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
	   {
			if(mos.IsAlive)
			{
				await PowerCmd.Apply<DazePower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
			}
	   }
    }
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
		if(dealer==null||dealer!=this.Owner.Creature)
		{
			await Task.CompletedTask;
			return;
		}
		if(target.HasPower<DazePower>())
		{
			await Task.CompletedTask;
			return;
		}
		await PowerCmd.Apply<DazePower>(target, this.DynamicVars["Power"].IntValue-1,Owner.Creature, this);
        await Task.CompletedTask;
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
