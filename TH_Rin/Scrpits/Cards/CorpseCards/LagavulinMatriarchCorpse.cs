using System.Runtime.Serialization.Json;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
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
public class LagavulinMatriarchCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<StrengthPower>(),
		  HoverTipFactory.FromPower<DexterityPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public LagavulinMatriarchCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 2m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
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
		if (dealer==null || dealer!=base.Owner.Creature)
		{
			await Task.CompletedTask;
			return;
		}
        await PowerCmd.Apply<StrengthPower>(Owner.Creature, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
		await PowerCmd.Apply<DexterityPower>(Owner.Creature, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
	    await PowerCmd.Apply<StrengthPower>(target, -this.DynamicVars["Power"].IntValue,Owner.Creature, this);
		await PowerCmd.Apply<DexterityPower>(target, -this.DynamicVars["Power"].IntValue,Owner.Creature, this);
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
