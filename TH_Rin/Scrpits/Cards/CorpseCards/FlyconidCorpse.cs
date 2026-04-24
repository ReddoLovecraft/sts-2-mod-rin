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
public class FlyconidCorpse : CorpseCardModel
{
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromPower<VulnerablePower>(),
          HoverTipFactory.FromPower<WeakPower>(),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public FlyconidCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 1m * GetMutilplier();
		DynamicVars.Cards.BaseValue = RotCount;
	}
	 public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
          if (player != base.Owner)
            {
                return;
            }
			CombatState combatState = player.Creature.CombatState;
			foreach(Creature mos in combatState.HittableEnemies)
			{
					if(mos.IsAlive)
					{
						if (combatState.RoundNumber %2 == 0)
						await PowerCmd.Apply<VulnerablePower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
						else
						await PowerCmd.Apply<WeakPower>(mos, this.DynamicVars["Power"].IntValue,Owner.Creature, this);
					}
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
