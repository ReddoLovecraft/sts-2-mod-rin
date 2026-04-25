using BaseLib.Extensions;
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
public class GhostTown : RinCardModel
{
	public override bool GainsBlock => true;
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[1]
        {
          HoverTipFactory.FromPower<WraithPower>()
        });
    protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(2m,ValueProp.Move),new CardsVar(3)];
	public GhostTown() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int add=Owner.HasPower<WraithPower>()?Owner.Creature.GetPowerAmount<WraithPower>():0;
		await PowerCmd.Apply<WraithPower>(Owner.Creature,2,Owner.Creature,this);
		for(int i=0;i<this.DynamicVars.Cards.IntValue;i++)
			await CreatureCmd.GainBlock(Owner.Creature,new BlockVar(this.DynamicVars.Block.IntValue+add,ValueProp.Move),cardPlay);
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(2);
	}
}

}
