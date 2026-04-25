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
public class PhantomBreathTechnique : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<EnragePower>()
        });
  	protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)];
	public PhantomBreathTechnique() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await PowerCmd.Apply<EnragePower>(Owner.Creature,this.DynamicVars.Cards.IntValue,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(2);
	}
}

}
