using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class NiddleMountian : RinCardModel
{
	public NiddleMountian() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int addtionRepeat= 1 ;
		int addtionMuti= 1 ;
		foreach(Creature mos in base.CombatState.HittableEnemies)
		{
		    if(mos.IsAlive)
			{
				addtionRepeat+=Scripts.Main.Tools.GetDebuffTotalCount(mos);
				addtionMuti+=Scripts.Main.Tools.GetDebuffKind(mos);
			}
		}
		SfxCmd.Play("event:/sfx/characters/silent/silent_dagger_spray");
		NDaggerSprayFlurryVfx.Create(base.Owner.Creature, new Color("rgba(243, 186, 154, 1)"), goingRight: true);
		IReadOnlyList<Creature> hittableEnemies = base.CombatState.HittableEnemies;
				foreach (Creature item in hittableEnemies)
				{
					NDaggerSprayImpactVfx child = NDaggerSprayImpactVfx.Create(item, new Color("#ffd1f1ff"), goingRight: true);
					NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(child);
				}
		for(int i=0;i<addtionRepeat;i++)
		{
	    	await CreatureCmd.Damage(choiceContext,base.CombatState.HittableEnemies,new DamageVar(addtionMuti,ValueProp.Unblockable|ValueProp.Unpowered),Owner.Creature);
		}
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
