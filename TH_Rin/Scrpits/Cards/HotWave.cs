using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class HotWave : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<IgnitePower>(),
          HoverTipFactory.FromPower<StrengthPower>()
        });
  protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(5)];
	public HotWave() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow");
		await Cmd.Wait(0.5f);
		NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NHorizontalLinesVfx.Create(new Color("ae140c80"), 1.2000000476837158, movingRightwards: false));
		await Cmd.Wait(0.5f);
		NCombatRoom.Instance?.RadialBlur(VfxPosition.Left);
		VfxCmd.PlayOnCreatureCenters(base.CombatState.HittableEnemies, "vfx/vfx_attack_blunt");
		using (IEnumerator<Creature> enumerator = base.CombatState.HittableEnemies.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Creature current = enumerator.Current;
				NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NLineBurstVfx.Create(current));
			}
		}
		NGame.Instance?.ScreenShake(ShakeStrength.Strong, ShakeDuration.Normal, 180f + MegaCrit.Sts2.Core.Random.Rng.Chaotic.NextFloat(-10f, 10f));
		foreach(Creature creature in base.CombatState.HittableEnemies)
		{
			if(creature.HasPower<IgnitePower>())
			{
				await PowerCmd.Apply<StrengthPower>(creature,-creature.GetPowerAmount<IgnitePower>(),Owner.Creature,this);
			}
		}
		await Cmd.Wait(0.2f);
		NGame.Instance?.DoHitStop(ShakeStrength.Strong, ShakeDuration.Normal);
		SfxCmd.Play("event:/sfx/enemy/enemy_attacks/ceremonial_beast/ceremonial_beast_plow_end");
		await PowerCmd.Apply<IgnitePower>(base.CombatState.HittableEnemies,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
	}
		
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(3);
	}
}

}
