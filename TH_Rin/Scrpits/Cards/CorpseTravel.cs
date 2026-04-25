using BaseLib.Extensions;
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
public class CorpseTravel : RinCardModel
{
	   public override bool GainsBlock => true;
	   public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Corpse"),
          Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Rot")
        });
	public CorpseTravel() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		IReadOnlyList<Creature> enemies = base.CombatState.HittableEnemies;
		foreach (Creature item in enemies)
		{
			NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NSpikeSplashVfx.Create(item));
		}
		int value=0;
		if(Owner.GetRelic<Barrow>()!=null)
		{
			value=Owner.GetRelic<Barrow>().CorpseCards.Count;
		}
		await DamageCmd.Attack(value).FromCard(this).TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_heavy_blunt", null, "blunt_attack.mp3")
			.WithHitVfxSpawnedAtBase()
			.Execute(choiceContext);
		await PowerCmd.Apply<PreventRotPower>(enemies, 1, base.Owner.Creature, this);
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
