using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class SoulAlchemy : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
           HoverTipFactory.FromPower<WraithPower>(),
		   HoverTipFactory.FromPotion<GhostInAJar>()
        });
	protected override bool IsPlayable => Owner.Creature.GetPowerAmount<WraithPower>()>5;
	public SoulAlchemy() : base(1, CardType.Skill, CardRarity.Rare, TargetType.None)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		if(Owner.HasPower<WraithPower>())
		{
			await Owner.Creature.GetPower<WraithPower>().TriggerExhasut(6);
			await PotionCmd.TryToProcure(ModelDb.Potion<GhostInAJar>().ToMutable(), base.Owner);
		}
	}
	protected override void OnUpgrade()
	{
		this.EnergyCost.UpgradeBy(-1);
	}
}

}
