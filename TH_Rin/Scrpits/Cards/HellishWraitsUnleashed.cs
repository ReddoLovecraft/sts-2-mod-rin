using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class HellishWraitsUnleashed : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<WraithPower>()
        });
	protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<WraithPower>();
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(1),new DamageVar(10m,ValueProp.Move)];
	public HellishWraitsUnleashed() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IReadOnlyList<Creature> enemies = base.CombatState.HittableEnemies;
		if(ShouldGlowGoldInternal)
		{
			int amount=Owner.Creature.GetPowerAmount<WraithPower>();
			for(int i=0;i<amount;i++)
			{
				await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
				.WithHitFx("vfx/vfx_starry_impact")
				.SpawningHitVfxOnEachCreature()
				.Execute(choiceContext);
			}
		}
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_starry_impact")
			.SpawningHitVfxOnEachCreature()
			.Execute(choiceContext);
		
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2m);
	}
}

}
