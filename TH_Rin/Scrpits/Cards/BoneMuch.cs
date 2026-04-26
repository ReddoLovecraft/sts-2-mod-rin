using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class BoneMuch : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Corpse")
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6m,ValueProp.Move)];
	public BoneMuch() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		int amount=1;
		if(Owner.GetRelic<Barrow>()!=null)
		    amount+=Owner.GetRelic<Barrow>().CorpseCards.Count;
		for(int i=0;i<amount;i++)
		{
			IReadOnlyList<Creature> enemies = base.CombatState.HittableEnemies;
			foreach (Creature item in enemies)
			{
			NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NSpikeSplashVfx.Create(item));
			}
			await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).WithHitCount(1).TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_heavy_blunt", null, "blunt_attack.mp3")
			.WithHitVfxSpawnedAtBase()
			.Execute(choiceContext);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2m);
	}
}

}
