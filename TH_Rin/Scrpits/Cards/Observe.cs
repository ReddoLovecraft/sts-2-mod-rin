using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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
public class Observe : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<StrengthPower>(),
          HoverTipFactory.FromPower<DexterityPower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public Observe() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if(cardPlay.Target.Monster.IntendsToAttack)
		{
			await PowerCmd.Apply<DexterityPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		}
		else
		{
			await PowerCmd.Apply<StrengthPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
	}
}

}
