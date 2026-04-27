using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using Patchoulib.Scrpits.Main;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class CorpseStreet : RinCardModel
{
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[2]
        {
          HoverTipFactory.FromPower<WraithPower>(),
		  Patchoulib.Scrpits.Main.Tools.GetStaticKeyword("Corpse")
        });
	public CorpseStreet() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int value=0;
		IBarrowLike? barrow = TH_Rin.Scripts.Main.Tools.GetBarrowRelic(Owner);
		if (barrow != null)
		{
			value=barrow.CorpseCards.Count;
			await PowerCmd.Apply<WraithPower>(Owner.Creature,value,Owner.Creature,this);
		}
	}
	protected override void OnUpgrade()
	{
		this.AddKeyword(CardKeyword.Innate);
	}
}

}
