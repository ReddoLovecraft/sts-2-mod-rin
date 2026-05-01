using System.ComponentModel;
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
public class SoulFyshCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<IntangiblePower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new HpLossVar(0)};
	public SoulFyshCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.HpLoss.BaseValue = 6m * GetMutilplier();
		DynamicVars["Power"].BaseValue = 1m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
	public override async Task AfterCardDrawnEarly(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
	{
		if (card.Owner == base.Owner && card.Type==CardType.Status)
		{
			Creature creature = base.Owner.RunState.Rng.CombatTargets.NextItem(base.Owner.Creature.CombatState.HittableEnemies);
			if (creature != null)
			{
			  await CreatureCmd.Damage(choiceContext,creature, DynamicVars.HpLoss.IntValue,ValueProp.Unpowered|ValueProp.Unblockable,null,null);
			}
		}
	}
		 public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
          if (player != base.Owner)
            {
                return;
            }
			CombatState combatState = player.Creature.CombatState;
			if (combatState.RoundNumber % 3 == 0)
			{
				await PowerCmd.Apply<IntangiblePower>(Owner.Creature, this.DynamicVars["Power"].IntValue, Owner.Creature, this);
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
