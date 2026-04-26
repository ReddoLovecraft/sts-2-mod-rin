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
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class VineShamblerCorpse : CorpseCardModel
{    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
	 public override bool GainsBlock => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0) };
	public VineShamblerCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.Cards.BaseValue = 3 * GetMutilplier();
	}
	  public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
  	{
          if (!CombatManager.Instance.IsInProgress)
          {
              await Task.CompletedTask;
              return;
          }
          if (target == null || target != Owner.Creature)
          {
              await Task.CompletedTask;
              return;
          }
          if(dealer==Owner.Creature)
          {
              await Task.CompletedTask;
              return;
          }
          await CreatureCmd.GainBlock(base.Owner.Creature, this.DynamicVars.Cards.IntValue,ValueProp.Unpowered, null);
 	 }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
