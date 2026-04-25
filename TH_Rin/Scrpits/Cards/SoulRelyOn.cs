using BaseLib.Extensions;
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
public class SoulRelyOn : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WraithPower>(),
          HoverTipFactory.FromPower<StrengthPower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2)];
	public SoulRelyOn() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<WraithPower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
		await PowerCmd.Apply<RelyOnPower>(Owner.Creature,1,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
	}
}

}
