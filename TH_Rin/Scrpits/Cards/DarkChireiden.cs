using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Powers;

namespace TH_Rin.Scrpits.Cards
{
[Pool(typeof(RinCardPool))]
public class DarkChireiden : RinCardModel
{
 	protected override IEnumerable<IHoverTip> ExtraHoverTips => (new IHoverTip[3]
        {
          HoverTipFactory.FromCard<RinKon>(base.IsUpgraded),
          HoverTipFactory.FromCard<Koishi>(base.IsUpgraded),
          HoverTipFactory.FromCard<Satori>(base.IsUpgraded),
        });
	public DarkChireiden() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		 List<CardModel> cards = new List<CardModel>
		 {
			base.CombatState.CreateCard<RinKon>(base.Owner),
			base.CombatState.CreateCard<Koishi>(base.Owner),
			base.CombatState.CreateCard<Satori>(base.Owner)
		 };
		 if (base.IsUpgraded)
		 {
			foreach (CardModel item in cards)
			{
				if (item.IsUpgradable)
				{
					CardCmd.Upgrade(item);
				}
			}
		 }
		CardModel cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, base.Owner, canSkip: false);
		if (cardModel != null)
		{
		 	await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
		}
	}
	protected override void OnUpgrade()
	{
		
	}
}

}
