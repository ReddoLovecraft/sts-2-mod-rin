using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TH_Rin.Relics;
using TH_Rin.Scrpits.Encounters;

namespace TH_Rin.Scrpits.Events;

public sealed class BlueStar : RinEventModel
{

	private const int JoinGold = 144;
	public override string? CustomInitialPortraitPath => "res://TH_Rin/ArtWorks/Events/bluestar.png";
	public override bool IsShared => true;

	protected override IReadOnlyList<EventOption> GenerateInitialOptions()
	{
		return
		[
			CreateOption(Join, "TH_RIN-BLUE_STAR.pages.INITIAL.options.JOIN"),
			CreateOption(Hunt, "TH_RIN-BLUE_STAR.pages.INITIAL.options.HUNT",
			[
				.. HoverTipFactory.FromRelic(ModelDb.Relic<HeroProof>())
			]),
		];
	}
    public override bool IsAllowed(IRunState runState)
	{
		return runState.CurrentActIndex ==3;
	}
	private async Task Join()
	{
		await PlayerCmd.GainGold(JoinGold, Owner!);
		SetEventFinished(PageDescription("JOIN"));
	}

	private Task Hunt()
	{
		EnterCombatWithoutExitingEvent<BlackDragonEventEncounter>(Array.Empty<Reward>(), shouldResumeAfterCombat: true);
		return Task.CompletedTask;
	}

	public override async Task Resume(AbstractRoom room)
	{
		if (Owner?.Creature == null || !Owner.Creature.IsAlive)
		{
			SetEventFinished(L10NLookup("TH_RIN-BLUE_STAR.loss"));
			return;
		}

		CombatRoom combatRoom = (CombatRoom)room;
		if (combatRoom.Encounter is not BlackDragonEventEncounter)
		{
			SetEventFinished(PageDescription("HUNT"));
			return;
		}

		await RelicCmd.Obtain(ModelDb.Relic<HeroProof>().ToMutable(), Owner);
		SetEventFinished(PageDescription("HUNT"));
	}
}
