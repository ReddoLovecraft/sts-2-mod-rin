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
public class TheLostCorpse : CorpseCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
           HoverTipFactory.FromPower<DexterityPower>()
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0) };
	public TheLostCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars.Cards.BaseValue = 3 * GetMutilplier();
	}
	public override async Task TriggerWhenTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
		if (player != base.Owner)
        {
                return;
        }
		foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		{
			if(mos.IsAlive)
			{
				await PowerCmd.Apply<DexterityPower>(mos,-DynamicVars.Cards.BaseValue,Owner.Creature,this);
				await PowerCmd.Apply<DexterityPower>(Owner.Creature,DynamicVars.Cards.BaseValue,Owner.Creature,this);
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

 