using BaseLib.Abstracts;
using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.Powers
{
    public sealed class DetectiveAssistantCatPower : CustomPowerModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;
        public override string? CustomPackedIconPath => "res://TH_Rin/ArtWorks/Powers/DACP32.png";
        public override string? CustomBigIconPath => "res://TH_Rin/ArtWorks/Powers/DACP64.png";
        public DetectiveAssistantCatPower() { }
         public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != base.Owner.Player)
            {
                return;
            }
	        CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 0,Amount);
		    List<CardModel> cardsIn = (from c in PileType.Draw.GetPile(base.Owner.Player).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		    if(cardsIn.Count>0)
		    foreach(CardModel card in await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, base.Owner.Player, prefs))
		    {
			if(card!=null)
			{
				card.EnergyCost.SetThisTurnOrUntilPlayed(0);
				await CardPileCmd.Add(card, PileType.Hand);
			} 
		    }
	        CardSelectorPrefs prefs2 = new CardSelectorPrefs(base.SelectionScreenPrompt, 0,Amount);
		    List<CardModel> cardsIn2 = (from c in PileType.Discard.GetPile(base.Owner.Player).Cards
			orderby c.Rarity, c.Id
			select c).ToList();
		    if(cardsIn2.Count>0)
		    foreach(CardModel card in await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn2, base.Owner.Player, prefs2))
		    {
			  if(card!=null)
				{
				card.EnergyCost.SetThisTurnOrUntilPlayed(0);
				await CardPileCmd.Add(card, PileType.Hand);
				} 
		    }
        }
	}

}