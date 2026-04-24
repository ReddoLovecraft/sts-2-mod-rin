using System.Runtime.Serialization.Json;
using BaseLib.Utils;
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
public class LivingShieldCorpse : CorpseCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
	public override bool GainsBlock => true;
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0) };
	public LivingShieldCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.Cards.BaseValue = 25 * GetMutilplier();
	}
	public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
		if (player != base.Owner)
        {
                return;
        }
		await CreatureCmd.GainBlock(Owner.Creature,new BlockVar(DynamicVars.Cards.BaseValue,ValueProp.Unpowered),null);
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}

 