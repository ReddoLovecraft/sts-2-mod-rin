using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using TH_Rin.Scripts.Main;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;

namespace TH_Rin.Scrpits.Cards;
[Pool(typeof(RinCardPool))]
public class CatMeeting : RinCardModel
{
	public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;
    
	public CatMeeting() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllAllies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IEnumerable<Creature> enumerable = from c in base.CombatState.GetTeammatesOf(base.Owner.Creature)
			where c != null && c.IsAlive && c.IsPlayer
			select c;
		foreach (Creature item in enumerable)
		{
			CardModel cardModel = CardFactory.GetDistinctForCombat(item.Player, from c in item.Player.Character.CardPool.GetUnlockedCards(item.Player.UnlockState, item.Player.RunState.CardMultiplayerConstraint)
			where c.Type == CardType.Skill||c.Type == CardType.Power||c.Type == CardType.Attack
			select c, 1, item.Player.RunState.Rng.CombatCardGeneration).FirstOrDefault();
			if (cardModel != null)
			{
				cardModel.SetToFreeThisTurn();
				await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
			}
		}

	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}
