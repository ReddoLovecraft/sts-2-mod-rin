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
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class InfestedPrismCorpse : CorpseCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
         this.EnergyHoverTip
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new EnergyVar(0)};
	public InfestedPrismCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.Energy.BaseValue = 2m * GetMutilplier();
	}
	 public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
	{
		if (!CombatManager.Instance.IsInProgress)
		{
			await Task.CompletedTask;
			return;
		}
		if (target != base.Owner.Creature)
		{
			await Task.CompletedTask;
			return;
		}
		if(dealer==null||dealer==this.Owner.Creature)
		{
			await Task.CompletedTask;
			return;
		}
		await PowerCmd.Apply<EnergyNextTurnPower>(Owner.Creature,this.DynamicVars.Energy.IntValue,null,null);
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
