using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using TH_Rin.Relics;
using TH_Rin.Scripts.Main;
using TH_Rin.Scrpits.Events;
namespace TH_Rin.Scrpits.Events
{
public sealed class CatCoffee : RinEventModel
{
	 public override string? CustomInitialPortraitPath => "res://TH_Rin/ArtWorks/Events/catcoffee.png";
	public override bool IsAllowed(IRunState runState)
	{
		return runState.Players.Count > 0 && runState.Players.All(p => p.Character is RinCharacter);
	}

	protected override IReadOnlyList<EventOption> GenerateInitialOptions()
	{
		return
		[
			CreateOption(Enter, "TH_RIN-CAT_COFFEE.pages.INITIAL.options.ENTER"),
			CreateOption(Ignore, "TH_RIN-CAT_COFFEE.pages.INITIAL.options.IGNORE"),
		];
	}

	private Task Ignore()
	{
		SetEventFinished(PageDescription("IGNORE"));
		return Task.CompletedTask;
	}

	private Task Enter()
	{
		int route = base.Rng.NextInt(3);
		if (route == 0)
		{
			SetEventState(PageDescription("SATORI"),
			[
				CreateOption(SatoriAhead, "TH_RIN-CAT_COFFEE.pages.SATORI.options.AHEAD",
				[
					.. HoverTipFactory.FromRelic(ModelDb.Relic<RoseMooncake>()),
					.. HoverTipFactory.FromCardWithCardHoverTips<Doubt>()
				]),
				CreateOption(SatoriWatch, "TH_RIN-CAT_COFFEE.pages.SATORI.options.WATCH")
			]);
			return Task.CompletedTask;
		}

		if (route == 1)
		{
			SetEventState(PageDescription("CHEN"),
			[
				CreateOption(ChenPull, "TH_RIN-CAT_COFFEE.pages.CHEN.options.PULL"),
				CreateOption(ChenTeach, "TH_RIN-CAT_COFFEE.pages.CHEN.options.TEACH")
			]);
			return Task.CompletedTask;
		}

		SetNothingPage();
		return Task.CompletedTask;
	}

	private async Task SatoriAhead()
	{
		await RelicCmd.Obtain(ModelDb.Relic<RoseMooncake>().ToMutable(), Owner!);
		await CardPileCmd.AddCursesToDeck(Enumerable.Repeat(ModelDb.Card<Doubt>(), 1), Owner!);
		SetEventFinished(PageDescription("AHEAD"));
	}

	private Task SatoriWatch()
	{
		IEnumerable<CardModel> upgradableCards = PileType.Deck.GetPile(Owner!).Cards.Where(c => c.IsUpgradable);
		foreach (CardModel card in upgradableCards.TakeRandom(4, base.Rng))
		{
			CardCmd.Upgrade(card);
		}
		SetEventFinished(PageDescription("WATCH"));
		return Task.CompletedTask;
	}

	private async Task ChenPull()
	{
		await PlayerCmd.GainGold(200, Owner!);
		SetEventFinished(PageDescription("PULL"));
	}

	private async Task ChenTeach()
	{
		RelicModel relic1 = RelicFactory.PullNextRelicFromFront(Owner!, RelicRarity.Rare).ToMutable();
		RelicModel relic2 = RelicFactory.PullNextRelicFromFront(Owner!, RelicRarity.Rare).ToMutable();
		await RelicCmd.Obtain(relic1, Owner!);
		await RelicCmd.Obtain(relic2, Owner!);
		SetEventFinished(PageDescription("TEACH"));
	}

	private void SetNothingPage()
	{
		SetEventState(PageDescription("NOTHING"),
		[
			CreateOption(Work, "TH_RIN-CAT_COFFEE.pages.NOTHING.options.WORK"),
			CreateOption(Back, "TH_RIN-CAT_COFFEE.pages.NOTHING.options.BACK")
		]);
	}

	private async Task Work()
	{
		await CreatureCmd.LoseMaxHp(new ThrowingPlayerChoiceContext(), Owner!.Creature, 1, false);
		await PlayerCmd.GainGold(20, Owner!);
		SetNothingPage();
	}

	private Task Back()
	{
		SetEventFinished(PageDescription("BACK"));
		return Task.CompletedTask;
	}
}
}
