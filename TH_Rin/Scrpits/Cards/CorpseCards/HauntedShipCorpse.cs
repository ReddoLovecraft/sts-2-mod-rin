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
public class HauntedShipCorpse : CorpseCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardModfier.NorotKeyword];
			protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<VulnerablePower>(),
           HoverTipFactory.FromPower<WeakPower>()
        });
	protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar> { new CardsVar(0),new DynamicVar("Power",0)};
	public HauntedShipCorpse() : base(-1, CardType.Status, CardRarity.Event, TargetType.None)
	{
		RefreshCorpseVars();
	}
	protected override void RefreshCorpseVars()
	{
		DynamicVars["Power"].BaseValue = 2m * GetMutilplier();
		DynamicVars.Cards.BaseValue = 200m * GetMutilplier();
	}
	 public override async Task TriggerWhenCombatStart()
        {
		   foreach(Creature mos in Owner.Creature.CombatState.HittableEnemies)
		   {
				if(!mos.IsAlive)
				{
					continue;
				}
				await PowerCmd.Apply<VulnerablePower>(mos, DynamicVars["Power"].BaseValue,mos,this);
				await PowerCmd.Apply<WeakPower>(mos, DynamicVars["Power"].BaseValue,mos,this);
		   }
        }
		public  override void TriggerWhenRemove()
    {
		Player owner = Owner;
		if (owner == null)
		{
			return;
		}
        PlayerCmd.GainGold(this.DynamicVars.Cards.IntValue, owner);
    }
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
