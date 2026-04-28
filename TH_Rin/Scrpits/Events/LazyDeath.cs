using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using TH_Rin.Scrpits.Cards;
using TH_Rin.Scrpits.Events;

namespace TH_Rin.Scrpits.Events
{
public sealed class LazyDeath : RinEventModel
{
	 public override string? CustomInitialPortraitPath => "res://TH_Rin/ArtWorks/Events/lazydeath.png";
	private static readonly IReadOnlyList<IHoverTip> ReportHoverTips = HoverTipFactory.FromCardWithCardHoverTips<Guilty>().ToList();
	private static readonly IReadOnlyList<IHoverTip> LearnHoverTips = HoverTipFactory.FromCardWithCardHoverTips<LazyTips>().ToList();

	public override bool IsAllowed(IRunState runState)
	{
		return runState.CurrentActIndex <= 1 && HasAnyRin(runState);
	}

	protected override IReadOnlyList<EventOption> GenerateInitialOptions()
	{
		return
		[
			CreateOption(Leave, "TH_RIN-LAZY_DEATH.pages.INITIAL.options.LEAVE"),
			CreateOption(Report, "TH_RIN-LAZY_DEATH.pages.INITIAL.options.REPORT", ReportHoverTips),
			CreateOption(Learn, "TH_RIN-LAZY_DEATH.pages.INITIAL.options.LEARN", LearnHoverTips),
		];
	}

	private Task Leave()
	{
		SetEventFinished(PageDescription("LEAVE"));
		return Task.CompletedTask;
	}

	private async Task Report()
	{
		await PlayerCmd.GainGold(200, Owner!);
		await CardPileCmd.AddCursesToDeck(Enumerable.Repeat(ModelDb.Card<Guilty>(), 1), Owner!);
		SetEventFinished(PageDescription("REPORT"));
	}

	private async Task Learn()
	{
		  CardModel card = base.Owner.RunState.CreateCard<LazyTips>(base.Owner);
		CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), 2f);
		SetEventFinished(PageDescription("LEARN"));
	}
}
}