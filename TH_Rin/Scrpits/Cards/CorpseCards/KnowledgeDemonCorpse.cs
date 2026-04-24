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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(StatusCardPool))]
public class KnowledgeDemonCorpse : CorpseCardModel
{
	public interface IChoosable
	{
		Task OnChosen();
	}
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
		protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          this.EnergyHoverTip
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0),new EnergyVar(0),new HpLossVar(0)};
	private int _energyCount;
	private int _drawCount;
	private int _damageCount;
	
	[SavedProperty]
	public int EnergyCount
	{
		get{return _energyCount;}
		set
		{
		AssertMutable();
		_energyCount=value;
		}
	}
	[SavedProperty]
	public int DrawCount
	{
		get{return _drawCount;}
		set
		{
		AssertMutable();
		_drawCount=value;
		}
	}
	[SavedProperty]
	public int DamageCount
	{
		get{return _damageCount;}
		set
		{
		AssertMutable();
		_damageCount=value;
		}
	}
	
	public KnowledgeDemonCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.Cards.BaseValue = 1;
		DynamicVars.Energy.BaseValue = 1;
		DynamicVars.HpLoss.BaseValue= 14 ;
	}
	
	public override async Task TriggerWhenCombatStart()
    {
		int totalChoices = EnergyCount + DrawCount + DamageCount;
		if (totalChoices >= 3)
		{
			return;
		}
		List<CardModel> choices = new List<CardModel>();
		CardModel energyCard = ModelDb.Card<Energy>().ToMutable();
		energyCard.Owner=Owner;
		CardModel drawCard = ModelDb.Card<Draw>().ToMutable();
		drawCard.Owner=Owner;
		CardModel damageCard = ModelDb.Card<Damage>().ToMutable();
		damageCard.Owner=Owner;
		choices.Add(energyCard);
		choices.Add(drawCard);
		choices.Add(damageCard);
		CardModel chosen = await CardSelectCmd.FromChooseACardScreen(new BlockingPlayerChoiceContext(), choices, Owner);
		if (chosen != null)
		{
			if (chosen is Energy)
			{
				EnergyCount++;
			}
			else if (chosen is Draw)
			{
				DrawCount++;
			}
			else if (chosen is Damage)
			{
				DamageCount++;
			}
		}
    }
	
	public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
	{
		if (EnergyCount > 0)
		{
			await PlayerCmd.GainEnergy(EnergyCount,player);
		}
		if (DrawCount > 0)
		{
			await CardPileCmd.Draw(choiceContext,DrawCount,player);
		}
	}
	
	public override async Task TriggerWhenTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
	{
		if (side != CombatSide.Player)
		{
			return;
		}
		if (DamageCount > 0)
		{
			int damage = 14 * DamageCount;
			await CreatureCmd.Damage(choiceContext,Owner.Creature.CombatState.HittableEnemies, new DamageVar(damage,ValueProp.Unpowered), null);
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
