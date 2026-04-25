using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class ChaosFire : RinCardModel
{
	private readonly Color _vfxTint = new Color("de4d4eff");
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<IgnitePower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(4), new RepeatVar(3)];
	public ChaosFire() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		Vector2 lastPos = Vector2.Zero;
		for (int i = 0; i < base.DynamicVars.Repeat.IntValue; i++)
		{
			Creature enemy = base.Owner.RunState.Rng.CombatTargets.NextItem(base.CombatState.HittableEnemies);
			if (enemy == null)
			{
				continue;
			}
			if (TestMode.IsOff)
			{
				if (i == 0)
				{
					lastPos = NCombatRoom.Instance.GetCreatureNode(base.Owner.Creature).VfxSpawnPosition;
				}
				NCreature targetNode = NCombatRoom.Instance.GetCreatureNode(enemy);
				if (targetNode != null)
				{
					NItemThrowVfx child = NItemThrowVfx.Create(lastPos, targetNode.GetBottomOfHitbox(), ModelDb.Potion<FirePotion>().Image);
					NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(child);
					lastPos = targetNode.VfxSpawnPosition;
					await Cmd.Wait(0.5f);
					NSplashVfx child2 = NSplashVfx.Create(targetNode.VfxSpawnPosition, _vfxTint);
					NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(child2);
					NLiquidOverlayVfx child3 = NLiquidOverlayVfx.Create(enemy, _vfxTint);
					NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(child3);
					NGaseousImpactVfx child4 = NGaseousImpactVfx.Create(targetNode.VfxSpawnPosition, _vfxTint);
					NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(child4);
				}
			}
			await PowerCmd.Apply<IgnitePower>(enemy, base.DynamicVars.Cards.BaseValue, base.Owner.Creature, this);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Repeat.UpgradeValueBy(1);
	}
}

}
