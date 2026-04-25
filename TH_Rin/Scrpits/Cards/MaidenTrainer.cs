using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class MaidenTrainer : RinCardModel
{
  protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(2),new BlockVar(8,ValueProp.Move)];
	public MaidenTrainer() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		await CreatureCmd.GainBlock(base.Owner.Creature, base.DynamicVars.Block, cardPlay);
		IEnumerable<CardModel> enumerable = PileType.Draw.GetPile(base.Owner).Cards.Where((CardModel c) => c.IsUpgradable).TakeRandom(base.DynamicVars.Cards.IntValue, base.Owner.RunState.Rng.CombatCardSelection);
		foreach (CardModel item in enumerable)
			{
				CardCmd.Upgrade(item);
				CardCmd.Preview(item);
			}
	}
	protected override void OnUpgrade()
	{
		DynamicVars.Cards.UpgradeValueBy(1);
		DynamicVars.Block.UpgradeValueBy(2);
	}
}

}
