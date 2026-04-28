using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using TH_Rin.Relics;
using TH_Rin.Scrpits.Events;
namespace TH_Rin.Scrpits.Events
{
public sealed class MysteriousCatCorpse : RinEventModel
{
	public override string? CustomInitialPortraitPath => "res://TH_Rin/ArtWorks/Events/mysteriouscatcorpse.png";
	private static readonly RelicModel[] PartRelics =
	[
		ModelDb.Relic<DeadCatEyes>(),
		ModelDb.Relic<DeadCatClaw>(),
		ModelDb.Relic<DeadCatHead>(),
		ModelDb.Relic<DeadCatTail>(),
	];

	protected override IEnumerable<DynamicVar> CanonicalVars =>
	[
		new StringVar("MissingRelic"),
		new StringVar("RemainRelic"),
	];

	public override bool IsAllowed(IRunState runState)
	{
		return runState.CurrentActIndex >= 1 && runState.CurrentActIndex <= 2;
	}

	protected override IReadOnlyList<EventOption> GenerateInitialOptions()
	{
		List<RelicModel> missingParts = GetMissingParts();
		SetStringVar("MissingRelic", JoinRelicTitles(missingParts));
		SetStringVar("RemainRelic", JoinRelicTitles(missingParts));
		List<IHoverTip> missingPartHoverTips = missingParts.SelectMany(HoverTipFactory.FromRelic).ToList();
		EventOption combineOption = missingParts.Count == 0
			? CreateOption(Combination, "TH_RIN-MYSTERIOUS_CAT_CORPSE.pages.INITIAL.options.COMBINATION",
			[
				.. HoverTipFactory.FromRelic(ModelDb.Relic<DeadCatCorpse>())
			])
			: CreateOption(null, "TH_RIN-MYSTERIOUS_CAT_CORPSE.pages.INITIAL.options.COMBINATION_LOCKED", Array.Empty<IHoverTip>());
		EventOption disassembleOption = CreateOption(Disassemble, "TH_RIN-MYSTERIOUS_CAT_CORPSE.pages.INITIAL.options.DISASSEMBLE", missingPartHoverTips);
		return [combineOption, disassembleOption];
	}

	private async Task Combination()
	{
		await RelicCmd.Obtain(ModelDb.Relic<DeadCatCorpse>().ToMutable(), Owner!);
		SetEventFinished(PageDescription("COMBINATION"));
	}

	private async Task Disassemble()
	{
		List<RelicModel> missingParts = GetMissingParts();
		foreach (RelicModel relic in missingParts)
		{
			await RelicCmd.Obtain(relic.ToMutable(), Owner!);
		}
		SetEventFinished(PageDescription("DISASSEMBLE"));
	}

	private List<RelicModel> GetMissingParts()
	{
		List<RelicModel> missing = [];
		if (Owner!.GetRelic<DeadCatEyes>() == null)
		{
			missing.Add(ModelDb.Relic<DeadCatEyes>());
		}
		if (Owner.GetRelic<DeadCatClaw>() == null)
		{
			missing.Add(ModelDb.Relic<DeadCatClaw>());
		}
		if (Owner.GetRelic<DeadCatHead>() == null)
		{
			missing.Add(ModelDb.Relic<DeadCatHead>());
		}
		if (Owner.GetRelic<DeadCatTail>() == null)
		{
			missing.Add(ModelDb.Relic<DeadCatTail>());
		}
		return missing;
	}

	private void SetStringVar(string key, string value)
	{
		((StringVar)DynamicVars[key]).StringValue = value;
	}

	private static string JoinRelicTitles(IEnumerable<RelicModel> relics)
	{
		List<string> names = relics.Select(r => r.Title.GetFormattedText()).ToList();
		if (names.Count == 0)
		{
			return "无";
		}
		return string.Join("、", names);
	}
}
}
