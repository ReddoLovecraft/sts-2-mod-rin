using System.Runtime.CompilerServices;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using TH_Rin.Scrpits.Relics;

namespace TH_Rin.Scrpits.UI;

public static class BarrowCorpseSelectContext
{
	private static readonly ConditionalWeakTable<NSimpleCardSelectScreen, IBarrowLike> _map = new ConditionalWeakTable<NSimpleCardSelectScreen, IBarrowLike>();

	public static void Attach(NSimpleCardSelectScreen screen, IBarrowLike barrow)
	{
		_map.Remove(screen);
		_map.Add(screen, barrow);
	}

	public static bool TryGet(NSimpleCardSelectScreen screen, out IBarrowLike barrow)
	{
		return _map.TryGetValue(screen, out barrow!);
	}
}
