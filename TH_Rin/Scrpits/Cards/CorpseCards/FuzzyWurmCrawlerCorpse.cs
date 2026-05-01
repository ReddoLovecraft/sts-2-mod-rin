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
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class FuzzyWurmCrawlerCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<StrengthPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public FuzzyWurmCrawlerCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 7m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
	public override async Task TriggerWhenTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (side == CombatSide.Enemy)
		{
			return;
		}

		Player owner = Owner;
		if (owner == null)
		{
			return;
		}

		CombatState? combatState = owner.Creature?.CombatState;
		if (combatState == null)
		{
			return;
		}

		int roundNumber = combatState.RoundNumber;
		bool playedAttackThisTurn = CombatManager.Instance.History.CardPlaysFinished.Any(e =>
			e.RoundNumber == roundNumber &&
			e.CurrentSide == CombatSide.Player &&
			e.Actor == owner.Creature &&
			e.CardPlay.Card.Type == CardType.Attack);

		if (!playedAttackThisTurn)
		{
			await PowerCmd.Apply<StrengthPower>(owner.Creature, this.DynamicVars["Power"].IntValue, owner.Creature, this);
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
