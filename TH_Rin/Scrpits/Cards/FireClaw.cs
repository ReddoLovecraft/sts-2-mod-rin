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
public class FireClaw : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<IgnitePower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(3,ValueProp.Move),new CardsVar(3)];
	public FireClaw() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this)
			.Targeting(cardPlay.Target)
			.BeforeDamage(delegate
			{
				NCombatRoom.Instance?.CombatVfxContainer.AddChildSafely(NGroundFireVfx.Create(cardPlay.Target));
				SfxCmd.Play("event:/sfx/characters/attack_fire");
				return Task.CompletedTask;
			})
			.Execute(choiceContext);
		await PowerCmd.Apply<IgnitePower>(cardPlay.Target,DynamicVars.Cards.IntValue,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Damage.UpgradeValueBy(2);
		DynamicVars.Cards.UpgradeValueBy(2);
	}
}

}
