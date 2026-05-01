using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
using System.Linq;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class LouseProgenitorCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<RitualPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public LouseProgenitorCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 20m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
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

		  bool alreadyTriggered = CombatManager.Instance.History.Entries
			  .OfType<DamageReceivedEntry>()
			  .Any(e => e.Receiver == Owner.Creature && e.Dealer != Owner.Creature);
		  if (alreadyTriggered)
		  {
			  await Task.CompletedTask;
			  return;
		  }
          await CreatureCmd.GainBlock(base.Owner.Creature,this.DynamicVars["Power"].IntValue,ValueProp.Unpowered, null);
 	 }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
