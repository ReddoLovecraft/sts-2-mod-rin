using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class SurfaceQuenching : RinCardModel
{
	public override bool GainsBlock => true;
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<IgnitePower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(8,ValueProp.Move)];
	public SurfaceQuenching() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
	 	await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		await PowerCmd.Apply<SurfaceQuenchingPower>(Owner.Creature,1,Owner.Creature,this);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(3);
	}
}

}
