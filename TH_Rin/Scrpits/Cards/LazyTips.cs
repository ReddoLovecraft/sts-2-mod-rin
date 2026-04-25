using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(EventCardPool))]
public class LazyTips : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<SlowPower>(),
          HoverTipFactory.FromPower<WeakPower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public LazyTips() : base(0, CardType.Skill, CardRarity.Event, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
	    await PowerCmd.Apply<SlowPower>(base.CombatState.HittableEnemies,1,Owner.Creature,this);
	    await PowerCmd.Apply<WeakPower>(base.CombatState.HittableEnemies,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		await PowerCmd.Apply<LazyTipsPower>(Owner.Creature,1,Owner.Creature,this);
		PlayerCmd.EndTurn(Owner, canBackOut: false);
	}
	protected override void OnUpgrade()
	{
		this.AddKeyword(CardKeyword.Retain);
	}
}

}
