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
public class Trust : RinCardModel
{
  public override bool GainsBlock => true;
  protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(9,ValueProp.Move)];
  public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Innate,CardKeyword.Retain];
	public Trust() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
	{
	}
	protected override bool ShouldGlowGoldInternal => Owner.Creature.Block<=0;

	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if(ShouldGlowGoldInternal)
	    await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
	}
	protected override PileType GetResultPileType()
	{
		PileType resultPileType = base.GetResultPileType();
		if (resultPileType != PileType.Discard)
		{
			return resultPileType;
		}
		return PileType.Hand;
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Block.UpgradeValueBy(3);
	}
}

}
