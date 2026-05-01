using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
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
using System.Linq;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class PhantasmalGardenerCorpse : CorpseCardModel
{
	 public override bool GainsBlock => true;
	 protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<StrengthPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new HpLossVar(0) };
	public PhantasmalGardenerCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 3 * GetMutilplier();
		DynamicVars.HpLoss.BaseValue = 7m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
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
		if(result.UnblockedDamage<=0)
		{
			await Task.CompletedTask;
			return;
		}
        await CreatureCmd.GainBlock(Owner.Creature, this.DynamicVars.HpLoss.IntValue,ValueProp.Unpowered,null);
        await Task.CompletedTask;
	}
	public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
          if (player != base.Owner)
            {
                return;
            }
			CombatState? combatState = player.Creature?.CombatState;
			if (combatState == null)
			{
				return;
			}

			int previousRoundNumber = combatState.RoundNumber - 1;
			if (previousRoundNumber < 1)
			{
				return;
			}

			bool tookUnblockedDamageLastRound = CombatManager.Instance.History.Entries
				.OfType<DamageReceivedEntry>()
				.Any(e => e.Receiver == player.Creature && e.RoundNumber == previousRoundNumber && e.Result.UnblockedDamage > 0);

			if (!tookUnblockedDamageLastRound)
			{
				await PowerCmd.Apply<StrengthPower>(base.Owner.Creature, this.DynamicVars["Power"].IntValue, base.Owner.Creature, this);
			}
			
    }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
