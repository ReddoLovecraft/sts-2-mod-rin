using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class HotHell : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<IgnitePower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(20)];
	public HotHell() : base(3, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
			float scale = 0.8f;
			NGroundFireVfx nGroundFireVfx = NGroundFireVfx.Create(Owner.Creature);
			if (nGroundFireVfx == null)
			{
					return;
			}
			SfxCmd.Play("event:/sfx/characters/attack_fire");
			nGroundFireVfx.Scale = Vector2.One * scale;
			NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(nGroundFireVfx);
			scale += 0.1f;
		await PowerCmd.Apply<IgnitePower>(Owner.Creature.CombatState.HittableEnemies,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		this.DynamicVars.Cards.UpgradeValueBy(10);
	}
}

}
