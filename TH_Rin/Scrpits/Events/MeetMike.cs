using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using TH_Rin.Relics;
using TH_Rin.Scrpits.Events;
using TH_Rin.Scrpits.Potions;
namespace TH_Rin.Scrpits.Events
{
public sealed class MeetMike : RinEventModel
{
	private const int Price = 200;
	public override string? CustomInitialPortraitPath => "res://TH_Rin/ArtWorks/Events/meetmike.png";
	private PotionModel? _commonPotion;
	private PotionModel? _uncommonPotion;
	private PotionModel? _rarePotion;

	public override bool IsAllowed(IRunState runState)
	{
		return HasAnyRin(runState) && runState.Players.All(p => p.Gold >= Price);
	}

	protected override IReadOnlyList<EventOption> GenerateInitialOptions()
	{
		_commonPotion = ModelDb.Potion<BottleOfWraith>().ToMutable();
		_uncommonPotion = base.Rng.NextItem(new PotionModel[]
		{
			ModelDb.Potion<CorpseBar>().ToMutable(),
			ModelDb.Potion<TrueFirePotion>().ToMutable(),
			ModelDb.Potion<NuclearFuel>().ToMutable()
		});
		_rarePotion = ModelDb.Potion<PreventingRotPotion>().ToMutable();
		return
		[
			CreateOption(BuyRelic, "TH_RIN-MEET_MIKE.pages.INITIAL.options.BUY_RELIC",
			[
				.. HoverTipFactory.FromRelic(ModelDb.Relic<PowerCardSoul>()),
				.. HoverTipFactory.FromRelic(ModelDb.Relic<PowerCardCar>())
			]),
			CreateOption(BuyPotion, "TH_RIN-MEET_MIKE.pages.INITIAL.options.BUY_POTION",
			[
				HoverTipFactory.FromPotion(_commonPotion),
				HoverTipFactory.FromPotion(_uncommonPotion),
				HoverTipFactory.FromPotion(_rarePotion)
			]),
		];
	}

	private async Task BuyRelic()
	{
		await PlayerCmd.LoseGold(Price, Owner!, GoldLossType.Stolen);
		await RelicCmd.Obtain(ModelDb.Relic<PowerCardSoul>().ToMutable(), Owner!);
		await RelicCmd.Obtain(ModelDb.Relic<PowerCardCar>().ToMutable(), Owner!);
		SetEventFinished(PageDescription("BUY_RELIC"));
	}

	private async Task BuyPotion()
	{
		await PlayerCmd.LoseGold(Price, Owner!, GoldLossType.Stolen);
		if (_commonPotion != null)
		{
			await PotionCmd.TryToProcure(_commonPotion, Owner!);
		}
		if (_uncommonPotion != null)
		{
			await PotionCmd.TryToProcure(_uncommonPotion, Owner!);
		}
		if (_rarePotion != null)
		{
			await PotionCmd.TryToProcure(_rarePotion, Owner!);
		}
		SetEventFinished(PageDescription("BUY_POTION"));
	}
}
}