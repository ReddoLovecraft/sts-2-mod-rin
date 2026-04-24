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
public class SlivingFogCorpse : CorpseCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new DynamicVar("Power",0)};
	public SlivingFogCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 6m * GetMutilplier();
	}
	private int dmg=0;
	 public override async Task TriggerWhenCombatStart()
        {
            dmg=this.DynamicVars["Power"].IntValue;
        }
		   public override async Task TriggerWhenTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
			  if (side != base.Owner.Creature.Side)
            {
                return;
            }
            await CreatureCmd.Damage(choiceContext, Owner.Creature.CombatState.HittableEnemies, new DamageVar(dmg,ValueProp.Unpowered), base.Owner.Creature);
			dmg+=this.DynamicVars["Power"].IntValue;
        }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
