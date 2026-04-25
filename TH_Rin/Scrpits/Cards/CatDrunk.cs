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
public class CatDrunk : RinCardModel
{
	 protected override bool HasEnergyCostX => true;
	public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
	public CatDrunk() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
	{
		
	}
	protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
	{
		await CreatureCmd.TriggerAnim(base.Owner.Creature, "Cast", base.Owner.Character.CastAnimDelay);
		int num = ResolveEnergyXValue();
		if(num>0)
		{
			 List<CardModel> cards = new List<CardModel>();
			CardPile drawPile = PileType.Draw.GetPile(base.Owner.Creature.Player);
			List<CardModel> items = drawPile.Cards.Where((CardModel c) => !c.Keywords.Contains(CardKeyword.Unplayable)).ToList();
			for(int i=0;i<num;i++)
			{
				CardModel cardModel = base.Owner.RunState.Rng.Shuffle.NextItem(items);
				if (cardModel != null)
				{
				  	cards.Add(cardModel);
				}
			}
			for(int i=cards.Count-1;i>=0;i--)
        	{
            	await CardPileCmd.Add(cards[i], PileType.Play);
        	}
         	foreach (CardModel item in cards)
			{
				await CardCmd.AutoPlay(choiceContext, item,null);
			}
		}
	}
	protected override void OnUpgrade()
	{
		this.RemoveKeyword(CardKeyword.Exhaust);
	}
}

}
