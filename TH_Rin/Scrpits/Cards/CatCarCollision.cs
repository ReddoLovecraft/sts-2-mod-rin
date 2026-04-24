using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class CatCarCollision : RinCardModel
{
	public override bool GainsBlock => true;
	protected override bool ShouldGlowGoldInternal => Owner.Creature.HasPower<CatCarPlayedPower>();
	public CatCarCollision() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		if(ShouldGlowGoldInternal&&Owner.HasPower<BlockNextTurnPower>())
		{
			await CreatureCmd.GainBlock(Owner.Creature,Owner.Creature.GetPowerAmount<BlockNextTurnPower>(),ValueProp.Unpowered,cardPlay);
		}
		int value=this.Owner.Creature.Block;
		await DamageCmd.Attack(value).FromCard(this).TargetingAllOpponents(base.CombatState)
			.WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
			.Execute(choiceContext);
		await PowerCmd.Apply<BlockNextTurnPower>(Owner.Creature,this.Owner.Creature.Block,Owner.Creature,this);
		if(!ShouldGlowGoldInternal)
		{
			await CreatureCmd.LoseBlock(Owner.Creature, Owner.Creature.Block);
		}
		await PowerCmd.Apply<CatCarPlayedPower>(Owner.Creature,1,null,null);
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
